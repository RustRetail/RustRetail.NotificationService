using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RustRetail.NotificationService.Application.Abstractions.Services.Email;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Repositories;

namespace RustRetail.NotificationService.Infrastructure.BackgroundJobs.HostedServices
{
    public class EmailNotificationSenderWorker : BackgroundService
    {
        readonly ILogger<EmailNotificationSenderWorker> _logger;
        readonly IServiceScopeFactory _scopeFactory;

        const int PollingIntervalInSeconds = 10;
        const int MaxRetries = 3;
        const int MaxBatchSize = 50;
        const int BaseRetryDelaySeconds = 2;

        public EmailNotificationSenderWorker(
            ILogger<EmailNotificationSenderWorker> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["Scope"] = nameof(EmailNotificationSenderWorker)
            }))
            {
                _logger.LogInformation("started at {Time}", DateTimeOffset.UtcNow);
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        await ProcessEmailBatchAsync(stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("EmailNotificationSenderWorker was cancelled");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception in EmailNotificationSenderWorker at {Time}", DateTimeOffset.UtcNow);
                    }

                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(PollingIntervalInSeconds), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("EmailNotificationSenderWorker delay was cancelled");
                        break;
                    }
                }
            }
        }

        private async Task ProcessEmailBatchAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Checking for pending email notifications at {Time}", DateTimeOffset.UtcNow);

            using var scope = _scopeFactory.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<INotificationUnitOfWork>();
            var notificationRepository = unitOfWork.GetRepository<INotificationRepository>();
            var contactRepository = unitOfWork.GetRepository<IUserContactInfoRepository>();
            var templateRepository = unitOfWork.GetRepository<INotificationTemplateRepository>();

            // Fetch and materialize pending email notifications
            var pendingEmailNotifications = (await notificationRepository
                .GetPendingEmailNotificationsAsync(MaxBatchSize, true, stoppingToken))
                .ToList();

            if (pendingEmailNotifications.Count == 0)
            {
                _logger.LogDebug("No pending email notifications found at {Time}", DateTimeOffset.UtcNow);
                return;
            }

            _logger.LogInformation("Found {Count} pending email notifications at {Time}",
                pendingEmailNotifications.Count, DateTimeOffset.UtcNow);

            // Process each email notification
            foreach (var notification in pendingEmailNotifications)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                await ProcessSingleNotificationAsync(notification, emailSender, contactRepository, templateRepository, stoppingToken);
            }

            // Save all changes to the database
            await SaveChangesAsync(unitOfWork, stoppingToken);
        }

        private async Task ProcessSingleNotificationAsync(
            Notification notification,
            IEmailService emailSender,
            IUserContactInfoRepository contactRepository,
            INotificationTemplateRepository templateRepository,
            CancellationToken stoppingToken)
        {
            var emailRecipients = notification.Recipients
                .Where(r => r.Channel == NotificationChannel.Email)
                .ToList();

            if (emailRecipients.Count == 0)
            {
                _logger.LogWarning("No email recipients found for notification {NotificationId}", notification.Id);
                return;
            }

            // Validate template first to avoid unnecessary processing
            if (notification.TemplateId == null || notification.TemplateId == Guid.Empty)
            {
                _logger.LogWarning("Email notification {NotificationId} has no template", notification.Id);
                UpdateRecipientsStatus(emailRecipients, NotificationStatus.Failed, "No template specified for email notification.");
                return;
            }

            var template = await templateRepository.GetByIdAsync(notification.TemplateId.Value, false, stoppingToken);
            if (template is null || !template.IsActive)
            {
                _logger.LogWarning("Email template {TemplateId} for notification {NotificationId} is not active or does not exist",
                    notification.TemplateId, notification.Id);
                UpdateRecipientsStatus(emailRecipients, NotificationStatus.Failed, "Email template is not active or does not exist.");
                return;
            }

            // Get all user contacts in one query and create lookup dictionary
            var userIds = emailRecipients.Select(r => r.UserId).ToList();
            var userContacts = await contactRepository.FindAsync(uc => userIds.Contains(uc.Id));
            var userContactsLookup = userContacts.ToDictionary(uc => uc.Id);

            // Process each recipient
            foreach (var recipient in emailRecipients)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                await ProcessRecipientAsync(recipient, userContactsLookup, template, emailSender, notification.Id, stoppingToken);
            }
        }

        private async Task ProcessRecipientAsync(
            NotificationRecipient recipient,
            Dictionary<Guid, UserContactInfo> userContactsLookup,
            NotificationTemplate template,
            IEmailService emailSender,
            Guid notificationId,
            CancellationToken stoppingToken)
        {
            // Validate recipient has contact info
            if (!userContactsLookup.TryGetValue(recipient.UserId, out var contactInfo))
            {
                _logger.LogWarning("No contact info found for user {UserId}", recipient.UserId);
                recipient.UpdateStatus(NotificationStatus.Failed, "User contact info not found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(contactInfo.Email))
            {
                _logger.LogWarning("No email address found for user {UserId}", recipient.UserId);
                recipient.UpdateStatus(NotificationStatus.Failed, "User email address not found.");
                return;
            }

            // Render email content with error handling
            string renderedBody;
            try
            {
                renderedBody = template.RenderBodyWithUserContact(contactInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to render email template for user {UserId}, notification {NotificationId}",
                    recipient.UserId, notificationId);
                recipient.UpdateStatus(NotificationStatus.Failed, $"Template rendering failed: {ex.Message}");
                return;
            }

            // Send email with retry logic
            await SendEmailWithRetriesAsync(recipient, contactInfo, template, renderedBody, emailSender, notificationId, stoppingToken);
        }

        private async Task SendEmailWithRetriesAsync(
            NotificationRecipient recipient,
            UserContactInfo contactInfo,
            NotificationTemplate template,
            string renderedBody,
            IEmailService emailSender,
            Guid notificationId,
            CancellationToken stoppingToken)
        {
            int attemptCount = 0;
            var maxAttempts = MaxRetries;
            while (attemptCount < maxAttempts && !stoppingToken.IsCancellationRequested)
            {
                attemptCount++;
                recipient.LastAttemptAt = DateTimeOffset.UtcNow;
                _logger.LogInformation("Sending email to {Email} for notification {NotificationId} - Attempt {Attempt}/{MaxAttempts}",
                    contactInfo.Email, notificationId, attemptCount, maxAttempts);
                try
                {
                    await emailSender.SendEmailAsync(
                        [contactInfo.Email],
                        template.Subject,
                        renderedBody,
                        cancellationToken: stoppingToken);
                    recipient.UpdateStatus(NotificationStatus.Sent, null, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
                    _logger.LogInformation("Email sent successfully to {Email} for notification {NotificationId} at {@Time}.", contactInfo.Email, notificationId, DateTimeOffset.UtcNow);
                    // Exit loop on success
                    return;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Email sending was cancelled for {Email}", contactInfo.Email);
                    throw;
                }
                catch (Exception ex)
                {
                    recipient.FailureCount = attemptCount;
                    _logger.LogError(ex, "Failed to send email to {Email} for notification {NotificationId} - Attempt {Attempt}/{MaxAttempts}",
                        contactInfo.Email, notificationId, attemptCount, maxAttempts);
                    // If this was the last attempt, mark as failed
                    if (attemptCount >= maxAttempts)
                    {
                        recipient.UpdateStatus(NotificationStatus.Failed, ex.Message);
                        _logger.LogError("Max retries reached for email {Email} for notification {NotificationId}",
                            contactInfo.Email, notificationId);
                        return;
                    }

                    // Add exponential back-off delay before next retry
                    var delaySeconds = BaseRetryDelaySeconds * Math.Pow(2, attemptCount - 1);
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Retry delay was cancelled for {Email}", contactInfo.Email);
                        throw;
                    }
                }
            }
        }

        private async Task SaveChangesAsync(INotificationUnitOfWork unitOfWork, CancellationToken stoppingToken)
        {
            try
            {
                await unitOfWork.SaveChangesAsync(stoppingToken);
                _logger.LogDebug("Successfully updated recipient statuses at {Time}", DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save changes to the database at {Time}", DateTimeOffset.UtcNow);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailNotificationSenderWorker is shutting down.");
            await base.StopAsync(cancellationToken);
        }

        private void UpdateRecipientsStatus(IEnumerable<NotificationRecipient> recipients, NotificationStatus status, string? message = null)
        {
            foreach (var recipient in recipients)
            {
                recipient.UpdateStatus(status, message);
            }
        }
    }
}

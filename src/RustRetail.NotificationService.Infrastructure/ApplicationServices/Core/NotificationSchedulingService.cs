using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Domain.Entities;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Events.Notification.Email;
using RustRetail.NotificationService.Domain.Repositories;

namespace RustRetail.NotificationService.Infrastructure.ApplicationServices.Core
{
    internal class NotificationSchedulingService : INotificationSchedulingService
    {
        readonly INotificationUnitOfWork _unitOfWork;
        readonly INotificationRepository _notificationRepository;
        readonly INotificationTemplateRepository _templateRepository;

        public NotificationSchedulingService(INotificationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _notificationRepository = unitOfWork.GetRepository<INotificationRepository>();
            _templateRepository = unitOfWork.GetRepository<INotificationTemplateRepository>();
        }

        public async Task ScheduleNotificationAsync(
            NotificationSchedulingOption option,
            CancellationToken cancellationToken = default)
        {
            // Create notification
            var notification = Notification.Create(
                option.SenderId,
                option.Title,
                option.Message,
                option.Category,
                option.Subtype,
                option.ActionLink,
                option.ScheduledAt,
                option.EntityId,
                option.EntityType);
            // Populate and add recipients to notification
            HashSet<NotificationRecipient> recipients = new HashSet<NotificationRecipient>();
            foreach (var recipientId in option.RecipientIds)
            {
                var recipient = NotificationRecipient.Create(recipientId, option.Channel);
                recipients.Add(recipient);
            }
            notification.AddRecipients(recipients);
            // Find notification template
            // For now, only email notification has template
            if (option.TemplateName is not null)
            {
                var template = await _templateRepository
                    .GetByNameAsync(option.TemplateName, asTracking: true, cancellationToken: cancellationToken);
                if (template is not null)
                {
                    notification.SetTemplate(template);
                }
            }

            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            notification.SetNotificationDomainEvent(option.Channel);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task ScheduleSingleRecipientEmailNotificationAsync(NotificationSchedulingOption option, Dictionary<string, object>? valuePairs = null, CancellationToken cancellationToken = default)
        {
            if (option.Channel != NotificationChannel.Email)
            {
                throw new InvalidOperationException("Only email notification can be scheduled.");
            }
            if (option.RecipientIds.Count() > 1)
            {
                throw new InvalidOperationException("Only one recipient can receive this notification.");
            }
            ArgumentException.ThrowIfNullOrWhiteSpace(option.TemplateName);

            // Create notification
            var notification = Notification.Create(
                option.SenderId,
                option.Title,
                option.Message,
                option.Category,
                option.Subtype,
                option.ActionLink,
                option.ScheduledAt,
                option.EntityId,
                option.EntityType);
            var recipient = NotificationRecipient.Create(option.RecipientIds.First(), option.Channel);
            notification.AddRecipients([recipient]);
            // Find notification template
            var template = await _templateRepository
                    .GetByNameAsync(option.TemplateName, asTracking: true, cancellationToken: cancellationToken);
            ArgumentNullException.ThrowIfNull(template);
            notification.SetTemplate(template);

            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            notification.SetNotificationDomainEvent(new EmailNotificationWithParamCreated(notification.Id, valuePairs));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Domain.Entities;
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
        }
    }
}

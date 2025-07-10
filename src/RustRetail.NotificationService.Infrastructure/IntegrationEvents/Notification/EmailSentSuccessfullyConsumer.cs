using MassTransit;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.SharedContracts.IntegrationEvents.V1.NotificationService.Email;

namespace RustRetail.NotificationService.Infrastructure.IntegrationEvents.Notification
{
    internal class EmailSentSuccessfullyConsumer(
        INotificationUnitOfWork unitOfWork)
        : IConsumer<EmailSentSuccessfullyEvent>
    {
        readonly INotificationRepository _notificationRepository = unitOfWork.GetRepository<INotificationRepository>();
        public async Task Consume(ConsumeContext<EmailSentSuccessfullyEvent> context)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(
                context.Message.NotificationId,
                asTracking: true,
                asSplitQuery: true,
                cancellationToken: context.CancellationToken,
                n => n.Recipients);

            // Don't need to throw exception if notification is not found, just return
            if (notification is null)
            {
                return;
            }
            foreach (var recipient in notification.Recipients)
            {
                recipient.LastAttemptAt = context.Message.SentAt;
                recipient.UpdateStatus(NotificationStatus.Sent, sentAt: context.Message.SentAt, deliveredAt: context.Message.SentAt);
            }
            notification.SetUpdatedDateTime(DateTime.UtcNow);
            _notificationRepository.Update(notification);
            await unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}

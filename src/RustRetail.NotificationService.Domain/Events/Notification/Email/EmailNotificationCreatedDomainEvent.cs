using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.NotificationService.Domain.Events.Notification.Email
{
    public class EmailNotificationCreatedDomainEvent : DomainEvent
    {
        public Guid NotificationId { get; init; }

        public EmailNotificationCreatedDomainEvent(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}

using RustRetail.SharedKernel.Domain.Events.Domain;

namespace RustRetail.NotificationService.Domain.Events.Notification.Email
{
    public class EmailNotificationWithParamCreated : DomainEvent
    {
        public EmailNotificationWithParamCreated(Guid notificationId, Dictionary<string, object>? parameters = null)
        {
            NotificationId = notificationId;
            Parameters = parameters;
        }

        public Guid NotificationId { get; init; }
        public Dictionary<string, object>? Parameters { get; init; }
    }
}

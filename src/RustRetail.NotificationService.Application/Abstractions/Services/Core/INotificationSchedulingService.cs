using RustRetail.NotificationService.Domain.Enums;

namespace RustRetail.NotificationService.Application.Abstractions.Services.Core
{
    public interface INotificationSchedulingService
    {
        Task ScheduleNotificationAsync(NotificationSchedulingOption option, CancellationToken cancellationToken = default);
        Task ScheduleSingleRecipientEmailNotificationAsync(NotificationSchedulingOption option, Dictionary<string, object>? valuePairs = null, CancellationToken cancellationToken = default);
    }

    public class NotificationSchedulingOption
    {
        public string SenderId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationCategory Category { get; set; } = NotificationCategory.Account;
        public string Subtype { get; set; } = string.Empty;
        public NotificationChannel Channel { get; set; } = NotificationChannel.Email;
        public IEnumerable<Guid> RecipientIds { get; set; } = [];
        public string? TemplateName { get; set; } = null;
        public DateTimeOffset? ScheduledAt { get; set; } = null;
        public string? ActionLink { get; set; } = null;
        public Guid? EntityId { get; set; } = null;
        public string? EntityType { get; set; } = null;

        public static NotificationSchedulingOption CreateEmailNotificationOption(
            string senderId,
            string title,
            string message,
            NotificationCategory category,
            string subType,
            IEnumerable<Guid> recipientIds,
            string templateName,
            DateTimeOffset? scheduleAt = null,
            string? actionLink = null)
        {
            return new NotificationSchedulingOption()
            {
                SenderId = senderId,
                Title = title,
                Message = message,
                Category = category,
                Subtype = subType,
                Channel = NotificationChannel.Email,
                RecipientIds = recipientIds,
                TemplateName = templateName,
                ScheduledAt = scheduleAt,
                ActionLink = actionLink
            };
        }

        public NotificationSchedulingOption SetRelatedEntity(
            Guid entityId,
            string entityType)
        {
            EntityId = entityId;
            EntityType = entityType;
            return this;
        }
    }
}

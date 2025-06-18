using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedKernel.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class Notification
        : AggregateRoot<Guid>
    {
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
        [MaxLength(1024)]
        public string? ActionLink { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTimeOffset? ReadAt { get; set; }
        public DateTimeOffset? ScheduledAt { get; set; }
        public DateTimeOffset? DismissedAt { get; set; }
        [Required]
        public NotificationCategory Category { get; set; } = NotificationCategory.Account;
        [Required]
        public string Subtype { get; set; } = NotificationSubtype.Account_AccountCreated;
        public Guid? EntityId { get; set; }
        [MaxLength(256)]
        public string? EntityType { get; set; }

        public ICollection<NotificationRecipient> Recipients { get; set; } = [];
        public Guid? TemplateId { get; set; }
        public NotificationTemplate? Template { get; set; }
    }
}

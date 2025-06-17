using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedKernel.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class NotificationRecipient : IHasKey<Guid>
    {
        public Guid Id { get; set; }
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public NotificationChannel Channel { get; set; } = default!;
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        [MaxLength(1024)]
        public string? StatusMessage { get; set; }
        public DateTimeOffset? SentAt { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public int FailureCount { get; set; } = 0;
        public DateTimeOffset? LastAttemptAt { get; set; }
    }
}

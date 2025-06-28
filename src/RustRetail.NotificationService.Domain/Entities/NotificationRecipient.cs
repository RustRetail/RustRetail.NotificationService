using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedKernel.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class NotificationRecipient : IHasKey<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public NotificationChannel Channel { get; set; } = NotificationChannel.Email;
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        [MaxLength(1024)]
        public string? StatusMessage { get; set; }
        public DateTimeOffset? SentAt { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public int FailureCount { get; set; } = 0;
        public DateTimeOffset? LastAttemptAt { get; set; }

        [Required]
        public Guid NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;

        public static NotificationRecipient Create(
            Guid userId,
            NotificationChannel channel,
            string? statusMessage = null)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel), "Notification channel cannot be null.");
            }

            return new NotificationRecipient()
            {
                UserId = userId,
                Channel = channel,
                Status = NotificationStatus.Pending,
                StatusMessage = statusMessage,
                SentAt = null,
                DeliveredAt = null,
                FailureCount = 0,
                LastAttemptAt = null
            };
        }

        public void UpdateStatus(
            NotificationStatus status,
            string? statusMessage = null,
            DateTimeOffset? sentAt = null,
            DateTimeOffset? deliveredAt = null)
        {
            Status = status;
            StatusMessage = statusMessage;
            SentAt = sentAt;
            DeliveredAt = deliveredAt;
            if (status == NotificationStatus.Failed)
            {
                FailureCount++;
                LastAttemptAt = DateTimeOffset.UtcNow;
            }
        }
    }
}

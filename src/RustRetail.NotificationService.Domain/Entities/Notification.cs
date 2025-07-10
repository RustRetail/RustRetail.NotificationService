using RustRetail.NotificationService.Domain.Constants;
using RustRetail.NotificationService.Domain.Enums;
using RustRetail.NotificationService.Domain.Events.Notification.Email;
using RustRetail.SharedKernel.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class Notification
        : AggregateRoot<Guid>
    {
        // In case of special users like system or admin, it can be empty string or non-guid value.
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
        [MaxLength(1024)]
        public string? ActionLink { get; set; }
        public bool IsRead { get; set; } = false;
        // Can remove ReadAt because notification recipient has its own read status.
        public DateTimeOffset? ReadAt { get; set; }
        /// <summary>
        /// Timestamp for notifications that are scheduled to be sent at a later time.
        /// This is not the same as the time when the notification is created.
        /// </summary>
        public DateTimeOffset? ScheduledAt { get; set; }
        // Can remove DismissedAt because notification recipient has its own dismissed status.
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

        /// <summary>
        /// Create a new notification instance with required parameters.
        /// Notification instance does not contain any recipients or template.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="subtype"></param>
        /// <param name="actionLink"></param>
        /// <param name="scheduledAt"></param>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Notification Create(
            string userId,
            string title,
            string message,
            NotificationCategory category,
            string subtype,
            string? actionLink = null,
            DateTimeOffset? scheduledAt = null,
            Guid? entityId = null,
            string? entityType = null)
        {
            // Only validate userId not null. In case of special users like system or admin, it can be empty string or non-guid value.
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(subtype))
            {
                throw new ArgumentException("Subtype cannot be null or empty.", nameof(subtype));
            }

            return new Notification()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = title,
                Message = message,
                ActionLink = actionLink,
                IsRead = false,
                ReadAt = null,
                ScheduledAt = scheduledAt,
                DismissedAt = null,
                Category = category,
                Subtype = subtype,
                EntityId = entityId,
                EntityType = entityType,
                CreatedDateTime = DateTimeOffset.UtcNow
            };
        }

        public void AddRecipients(IEnumerable<NotificationRecipient> recipients)
        {
            if (recipients == null || !recipients.Any())
            {
                throw new ArgumentException("Recipients cannot be null or empty.", nameof(recipients));
            }
            foreach (var recipient in recipients)
            {
                if (recipient == null)
                {
                    throw new ArgumentNullException(nameof(recipient), "Recipient cannot be null.");
                }
                // Skip if recipient already exists in the collection.
                // Duplication is decided by recipient's Id and UserId.
                if (Recipients.Any(r => r.Id == recipient.Id || r.UserId == recipient.UserId))
                {
                    continue;
                }
                Recipients.Add(recipient);
            }
        }

        public void SetTemplate(NotificationTemplate? template)
        {
            if (template is null)
            {
                throw new ArgumentNullException(nameof(template), "Template cannot be null.");
            }
            // Only set notification's template id because of ef core change tracking.
            TemplateId = template.Id;
        }

        public void SetNotificationDomainEvent(NotificationChannel channel)
        {
            if (channel == NotificationChannel.Email)
            {
                AddDomainEvent(new EmailNotificationCreatedDomainEvent(Id));
            }
        }
    }
}

using RustRetail.SharedKernel.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class Notification
        : AggregateRoot<Guid>
    {
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ActionLink { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTimeOffset? ReadAt { get; set; }

        public DateTimeOffset? ScheduledAt { get; set; }

        public DateTimeOffset? DismissedAt { get; set; }
    }
}

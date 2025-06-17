using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedKernel.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class NotificationTemplate : AggregateRoot<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        public string? Footer { get; set; }
        public string? Header { get; set; }
        public string? DefaultActionLink { get; set; }
        public NotificationType Type { get; set; } = default!;
        public bool IsActive { get; set; } = true;
    }
}

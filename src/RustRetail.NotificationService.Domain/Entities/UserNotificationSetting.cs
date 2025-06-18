using RustRetail.NotificationService.Domain.Enums;
using RustRetail.SharedKernel.Domain.Models;

namespace RustRetail.NotificationService.Domain.Entities
{
    public sealed class UserNotificationSetting : AggregateRoot<Guid>
    {
        public Guid UserId { get; set; }
        public NotificationCategory Category { get; set; } = default!;
        public NotificationChannel Channel { get; set; } = default!;
        public bool IsEnabled { get; set; } = true;
    }
}

using RustRetail.SharedKernel.Domain.Enums;

namespace RustRetail.NotificationService.Domain.Enums
{
    public sealed class NotificationType : Enumeration
    {
        private NotificationType(string name, int value) : base(name, value) { }

        public static readonly NotificationType Account = new(nameof(Account), 0);
        public static readonly NotificationType Order = new(nameof(Order), 1);
    }
}

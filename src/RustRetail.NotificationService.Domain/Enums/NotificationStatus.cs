using RustRetail.SharedKernel.Domain.Enums;

namespace RustRetail.NotificationService.Domain.Enums
{
    public sealed class NotificationStatus : Enumeration
    {
        private NotificationStatus(string name, int value) : base(name, value) { }

        public static readonly NotificationStatus Pending = new(nameof(Pending), 0);
        public static readonly NotificationStatus Sent = new(nameof(Sent), 1);
        public static readonly NotificationStatus Failed = new(nameof(Failed), 2);
        public static readonly NotificationStatus Delivered = new(nameof(Delivered), 3);
        public static readonly NotificationStatus Viewed = new(nameof(Viewed), 4);
        public static readonly NotificationStatus Clicked = new(nameof(Clicked), 5);
    }
}

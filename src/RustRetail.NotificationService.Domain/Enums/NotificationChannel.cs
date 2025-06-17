using RustRetail.SharedKernel.Domain.Enums;

namespace RustRetail.NotificationService.Domain.Enums
{
    public sealed class NotificationChannel : Enumeration
    {
        private NotificationChannel(string name, int value) : base(name, value) { }

        public static readonly NotificationChannel InApp = new(nameof(InApp), 0);
        public static readonly NotificationChannel Email = new(nameof(Email), 1);
        public static readonly NotificationChannel Push = new(nameof(Push), 2);
        public static readonly NotificationChannel SMS = new(nameof(SMS), 3);
    }
}

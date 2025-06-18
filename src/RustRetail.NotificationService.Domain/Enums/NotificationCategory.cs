using RustRetail.SharedKernel.Domain.Enums;

namespace RustRetail.NotificationService.Domain.Enums
{
    public sealed class NotificationCategory : Enumeration
    {
        private NotificationCategory(string name, int value) : base(name, value) { }

        public static readonly NotificationCategory Account = new(nameof(Account), 0);
        public static readonly NotificationCategory Order = new(nameof(Order), 1);
        public static readonly NotificationCategory Marketing = new(nameof(Marketing), 2);
        public static readonly NotificationCategory System = new(nameof(System), 3);
        public static readonly NotificationCategory Service = new(nameof(Service), 4);
    }
}

namespace RustRetail.NotificationService.Domain.Constants
{
    public static class NotificationSubtype
    {
        // Account related notification subtypes
        public const string Account_AccountCreated = nameof(Account_AccountCreated);
        public const string Account_AccountUpdated = nameof(Account_AccountUpdated);
        public const string Account_AccountDeleted = nameof(Account_AccountDeleted);
        public const string Account_EmailVerificationRequested = nameof(Account_EmailVerificationRequested);
        public const string Account_EmailVerified = nameof(Account_EmailVerified);
        public const string Account_LockedOut = nameof(Account_LockedOut);

        // Order related notification subtypes
        public const string Order_OrderPlaced = nameof(Order_OrderPlaced);
        public const string Order_OrderConfirmed = nameof(Order_OrderConfirmed);
        public const string Order_OrderShipped = nameof(Order_OrderShipped);
        public const string Order_OrderDelivered = nameof(Order_OrderDelivered);
        public const string Order_OrderCancelled = nameof(Order_OrderCancelled);
        public const string Order_OrderRefunded = nameof(Order_OrderRefunded);
        public const string Order_OrderReturned = nameof(Order_OrderReturned);

    }
}

namespace RustRetail.NotificationService.Contracts.IntegrationEvents.User
{
    public sealed class UserRegisteredEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

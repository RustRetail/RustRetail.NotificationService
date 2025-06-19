namespace RustRetail.NotificationService.Contracts.IntegrationEvents
{
    public abstract class IntegrationEvent
    {
        public Guid Id { get; set; }

        public DateTime OccurredOn { get; set; }
    }
}

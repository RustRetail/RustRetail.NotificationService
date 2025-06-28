using MediatR;
using RustRetail.SharedContracts.IntegrationEvents.Abstractions;

namespace RustRetail.NotificationService.Application.Abstractions.Event
{
    public class IntegrationEventNotification<TIntegrationEvent>
        : INotification
        where TIntegrationEvent : IIntegrationEvent
    {
        public TIntegrationEvent Event { get; }

        public IntegrationEventNotification(TIntegrationEvent integrationEvent)
        {
            Event = integrationEvent;
        }
    }
}

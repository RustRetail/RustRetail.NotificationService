using MassTransit;
using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.User;

namespace RustRetail.NotificationService.Infrastructure.IntegrationEvents.User
{
    internal class UserRegisteredEventConsumer(
        IPublisher publisher)
        : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var notification = new IntegrationEventNotification<UserRegisteredEvent>(context.Message);
            await publisher.Publish(notification, context.CancellationToken);
        }
    }
}

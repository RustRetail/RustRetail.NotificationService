using MassTransit;
using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.Authentication;

namespace RustRetail.NotificationService.Infrastructure.IntegrationEvents.User
{
    internal class UserLockedOutEventConsumer(
        IPublisher publisher)
        : IConsumer<UserLockedOutEvent>
    {
        public async Task Consume(ConsumeContext<UserLockedOutEvent> context)
        {
            var notification = new IntegrationEventNotification<UserLockedOutEvent>(context.Message);
            await publisher.Publish(notification, context.CancellationToken);
        }
    }
}

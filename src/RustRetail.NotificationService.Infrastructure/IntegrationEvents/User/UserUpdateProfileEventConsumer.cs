using MassTransit;
using MediatR;
using RustRetail.NotificationService.Application.Abstractions.Event;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.User;

namespace RustRetail.NotificationService.Infrastructure.IntegrationEvents.User
{
    internal class UserUpdateProfileEventConsumer(
        IPublisher publisher)
        : IConsumer<UserUpdatedProfileEvent>
    {
        public async Task Consume(ConsumeContext<UserUpdatedProfileEvent> context)
        {
            var notification = new IntegrationEventNotification<UserUpdatedProfileEvent>(context.Message);
            await publisher.Publish(notification, context.CancellationToken);
        }
    }
}

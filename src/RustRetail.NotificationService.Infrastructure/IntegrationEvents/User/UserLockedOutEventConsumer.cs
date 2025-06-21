using MassTransit;
using Microsoft.Extensions.Logging;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.Authentication;

namespace RustRetail.NotificationService.Infrastructure.IntegrationEvents.User
{
    internal class UserLockedOutEventConsumer(
        ILogger<UserLockedOutEventConsumer> logger)
        : IConsumer<UserLockedOutEvent>
    {
        public Task Consume(ConsumeContext<UserLockedOutEvent> context)
        {
            logger.LogInformation(
                "User locked out: EventId={@EventId}, OccurredOn={@OccurredOn}, UserId={@UserId}, Duration={@Duration}",
                context.Message.Id,
                context.Message.OccurredOn,
                context.Message.UserId,
                context.Message.LockoutDurationInMilliseconds);

            return Task.CompletedTask;
        }
    }
}

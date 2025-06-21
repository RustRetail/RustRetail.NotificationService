using MassTransit;
using Microsoft.Extensions.Logging;
using RustRetail.SharedContracts.IntegrationEvents.V1.IdentityService.User;

namespace RustRetail.NotificationService.Infrastructure.IntegrationEvents.User
{
    internal class UserRegisteredEventConsumer(
        ILogger<UserRegisteredEventConsumer> logger)
        : IConsumer<UserRegisteredEvent>
    {
        public Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            logger.LogInformation(
                "User registered: EventId={@EventId}, OccurredOn={@OccurredOn}, UserId={@UserId}, UserName={@UserName}, Email={@Email}, RegisterAt={@RegisterAt}",
                context.Message.Id,
                context.Message.OccurredOn,
                context.Message.UserId,
                context.Message.UserName,
                context.Message.Email,
                context.Message.RegisteredAt);

            return Task.CompletedTask;
        }
    }
}

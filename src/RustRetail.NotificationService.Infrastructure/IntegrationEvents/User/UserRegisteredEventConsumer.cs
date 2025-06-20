using MassTransit;
using Microsoft.Extensions.Logging;
using RustRetail.NotificationService.Contracts.IntegrationEvents.User;

namespace RustRetail.NotificationService.Infrastructure.IntegrationEvents.User
{
    internal class UserRegisteredEventConsumer(
        ILogger<UserRegisteredEventConsumer> logger)
        : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            logger.LogInformation(
                "User registered: EventId={EventId}, OccurredOn={OccurredOn}, UserId={UserId}, UserName={UserName}, Email={Email}",
                context.Message.Id,
                context.Message.OccurredOn,
                context.Message.UserId,
                context.Message.UserName,
                context.Message.Email);
        }
    }
}

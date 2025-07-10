using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RustRetail.NotificationService.Infrastructure.IntegrationEvents.Notification;
using RustRetail.NotificationService.Infrastructure.IntegrationEvents.User;
using RustRetail.SharedApplication.Behaviors.Messaging;

namespace RustRetail.NotificationService.Infrastructure.MessageBrokers.RabbitMQ
{
    internal static class RabbitMQServiceCollectionExtensions
    {
        internal static IServiceCollection AddRabbitMQ(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<UserRegisteredEventConsumer>();
                x.AddConsumer<UserLockedOutEventConsumer>();
                x.AddConsumer<EmailSentSuccessfullyConsumer>();
                x.AddConsumer<EmailSentFailedConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = configuration.GetSection(RabbitMQOptions.SectionName).Get<RabbitMQOptions>();
                    ArgumentNullException.ThrowIfNull(options, nameof(options));
                    cfg.Host(options.Host, options.VirtualHost, h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IMessageBus, MassTransitMessageSender>();

            return services;
        }
    }
}

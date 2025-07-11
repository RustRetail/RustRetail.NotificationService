using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RustRetail.NotificationService.Infrastructure.ApplicationServices;
using RustRetail.NotificationService.Infrastructure.MessageBrokers.RabbitMQ;

namespace RustRetail.NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplicationServices();
            services.AddRabbitMQ(configuration);

            return services;
        }
    }
}

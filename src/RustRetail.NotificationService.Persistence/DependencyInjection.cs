using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.SharedPersistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace RustRetail.NotificationService.Persistence
{
    public static class DependencyInjection
    {
        const string ConnectionStringName = "NotificationDatabase";

        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddInterceptors()
                .AddDbContext(configuration);

            return services;
        }

        private static IServiceCollection AddDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<NotificationDbContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<DomainEventHandlingInterceptor>();
                options.UseNpgsql(configuration.GetConnectionString(ConnectionStringName));
                options.AddInterceptors(interceptor);
            });

            return services;
        }

        private static IServiceCollection AddInterceptors(
            this IServiceCollection services)
        {
            services.AddScoped<DomainEventHandlingInterceptor>();

            return services;
        }
    }
}

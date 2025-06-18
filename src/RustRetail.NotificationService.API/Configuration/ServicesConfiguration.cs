using RustRetail.NotificationService.API.Configuration.Json;

namespace RustRetail.NotificationService.API.Configuration
{
    internal static class ServicesConfiguration
    {
        internal static IServiceCollection AddApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddJsonConfiguration();

            return services;
        }

        internal static IServiceCollection AddSharedServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpContextAccessor()
                .AddOpenApi();

            return services;
        }
    }
}

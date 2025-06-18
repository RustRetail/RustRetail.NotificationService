namespace RustRetail.NotificationService.API.Configuration
{
    internal static class ServicesConfiguration
    {
        internal static IServiceCollection AddApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {

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

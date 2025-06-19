using RustRetail.NotificationService.Infrastructure.ApplicationServices.Email.Gmail;

namespace RustRetail.NotificationService.API.Configuration
{
    internal static class OptionsConfiguration
    {
        internal static IServiceCollection ConfiguringOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Infrastructure options
            services.Configure<GmailEmailOptions>(configuration.GetSection(GmailEmailOptions.SectionName));

            return services;
        }
    }
}

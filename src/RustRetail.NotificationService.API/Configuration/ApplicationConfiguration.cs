using RustRetail.SharedInfrastructure.Logging.Serilog;

namespace RustRetail.NotificationService.API.Configuration
{
    internal static class ApplicationConfiguration
    {
        internal static WebApplication ConfigureApplicationPipeline(
            this WebApplication app)
        {
            app.UseExceptionHandler();
            app.UseSharedSerilogRequestLogging();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}

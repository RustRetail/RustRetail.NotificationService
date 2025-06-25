using RustRetail.SharedInfrastructure.Logging.Serilog;

namespace RustRetail.NotificationService.API.Configuration
{
    internal static class ApplicationConfiguration
    {
        internal static WebApplication ConfigureApplicationPipeline(
            this WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseExceptionHandler();

            app.UseSharedSerilogRequestLogging();

            return app;
        }
    }
}

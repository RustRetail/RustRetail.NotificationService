namespace RustRetail.NotificationService.API.Configuration
{
    internal static class ApplicationConfiguration
    {
        internal static WebApplication ConfigureApplicationPipeline(
            this WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseExceptionHandler();

            return app;
        }
    }
}

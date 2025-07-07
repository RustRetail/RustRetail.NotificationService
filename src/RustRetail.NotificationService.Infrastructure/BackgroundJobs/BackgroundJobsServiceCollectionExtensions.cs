using Microsoft.Extensions.DependencyInjection;
using RustRetail.NotificationService.Infrastructure.BackgroundJobs.HostedServices;
using System.Runtime.CompilerServices;

namespace RustRetail.NotificationService.Infrastructure.BackgroundJobs
{
    internal static class BackgroundJobsServiceCollectionExtensions
    {
        internal static IServiceCollection AddBackgroundJobs(
            this IServiceCollection services)
        {
            // Add hosted services for background jobs
            services.AddHostedService<EmailNotificationSenderWorker>();

            return services;
        }
    }
}

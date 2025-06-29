using Microsoft.Extensions.DependencyInjection;
using RustRetail.NotificationService.Application.Abstractions.Services.Core;
using RustRetail.NotificationService.Application.Abstractions.Services.Email;
using RustRetail.NotificationService.Infrastructure.ApplicationServices.Core;
using RustRetail.NotificationService.Infrastructure.ApplicationServices.Email.Gmail;

namespace RustRetail.NotificationService.Infrastructure.ApplicationServices
{
    internal static class ApplicationServicesCollectionExtensions
    {
        internal static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IEmailService, GmailEmailService>();
            services.AddScoped<IUserContactInfoService, UserContactInfoService>();
            services.AddScoped<INotificationSchedulingService, NotificationSchedulingService>();

            return services;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RustRetail.NotificationService.Domain.Repositories;
using RustRetail.NotificationService.Persistence.Database;
using RustRetail.NotificationService.Persistence.Repositories;
using RustRetail.SharedKernel.Domain.Abstractions;
using RustRetail.SharedPersistence.Database;
using RustRetail.SharedPersistence.Interceptors;

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
                .AddDbContext(configuration)
                .AddUnitOfWork()
                .AddRepositories();

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

        private static IServiceCollection AddUnitOfWork(
            this IServiceCollection services)
        {
            services.AddScoped<INotificationUnitOfWork, NotificationUnitOfWork>();

            return services;
        }

        private static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            // Generic repository
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            // Custom repository
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();
            services.AddScoped<IUserContactInfoRepository, UserContactInfoRepository>();
            services.AddScoped<IUserNotificationSettingRepository, UserNotificationSettingRepository>();

            return services;
        }
    }
}

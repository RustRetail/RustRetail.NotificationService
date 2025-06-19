using Microsoft.AspNetCore.Http.Features;
using RustRetail.NotificationService.API.Configuration.Json;
using RustRetail.NotificationService.API.Middlewares;
using System.Diagnostics;

namespace RustRetail.NotificationService.API.Configuration
{
    internal static class ServicesConfiguration
    {
        internal static IServiceCollection AddApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddJsonConfiguration()
                .AddGlobalExceptionHandling();

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

        private static IServiceCollection AddGlobalExceptionHandling(
            this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (context) =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
                };
            });

            return services;
        }
    }
}

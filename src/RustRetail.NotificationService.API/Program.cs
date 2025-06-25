using RustRetail.NotificationService.API.Configuration;
using RustRetail.NotificationService.Application;
using RustRetail.NotificationService.Infrastructure;
using RustRetail.NotificationService.Persistence;
using RustRetail.SharedInfrastructure.Logging.Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfiguringOptions(builder.Configuration)
    .AddSharedServices(builder.Configuration)
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApi(builder.Configuration);
builder.Host.UseSharedSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ConfigureApplicationPipeline();

app.Run();

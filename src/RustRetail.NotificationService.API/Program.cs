using RustRetail.NotificationService.API.Configuration;
using RustRetail.NotificationService.Application;
using RustRetail.NotificationService.Infrastructure;
using RustRetail.NotificationService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfiguringOptions(builder.Configuration)
    .AddSharedServices(builder.Configuration)
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApi(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RustRetail.NotificationService.API/RustRetail.NotificationService.API.csproj", "src/RustRetail.NotificationService.API/"]
COPY ["src/RustRetail.NotificationService.Contracts/RustRetail.NotificationService.Contracts.csproj", "src/RustRetail.NotificationService.Contracts/"]
COPY ["src/RustRetail.NotificationService.Infrastructure/RustRetail.NotificationService.Infrastructure.csproj", "src/RustRetail.NotificationService.Infrastructure/"]
COPY ["src/RustRetail.NotificationService.Application/RustRetail.NotificationService.Application.csproj", "src/RustRetail.NotificationService.Application/"]
COPY ["src/RustRetail.NotificationService.Persistence/RustRetail.NotificationService.Persistence.csproj", "src/RustRetail.NotificationService.Persistence/"]
COPY ["src/RustRetail.NotificationService.Domain/RustRetail.NotificationService.Domain.csproj", "src/RustRetail.NotificationService.Domain/"]
RUN dotnet restore "./src/RustRetail.NotificationService.API/RustRetail.NotificationService.API.csproj"
COPY . .
WORKDIR "/src/src/RustRetail.NotificationService.API"
RUN dotnet build "./RustRetail.NotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RustRetail.NotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RustRetail.NotificationService.API.dll"]
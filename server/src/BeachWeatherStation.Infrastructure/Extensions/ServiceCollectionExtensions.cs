using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Infrastructure.Data;
using BeachWeatherStation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeachWeatherStation.Infrastructure.Extensions;

// Extension methods for registering infrastructure services in DI
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers infrastructure services, repositories, and DbContext for BeachWeatherStation.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Cosmos DB context
        services.AddDbContext<BeachWeatherStationDbContext>(options =>
            options.UseCosmos(
                configuration["CosmosDb:AccountEndpoint"],
                configuration["CosmosDb:AccountKey"],
                databaseName: configuration["CosmosDb:DatabaseName"]));

        // Register repositories and services
        services.AddScoped<IBatteryChangeRepository, BatteryChangeRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IHeartbeatRepository, HeartbeatRepository>();
        services.AddScoped<ITemperatureReadingRepository, TemperatureReadingRepository>();

        services.AddScoped<Services.AlertService>();
        return services;
    }
}

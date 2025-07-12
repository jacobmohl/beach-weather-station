using Microsoft.Extensions.DependencyInjection;
using BeachWeatherStation.Application.Services;
using BeachWeatherStation.Application.Validators;

namespace BeachWeatherStation.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Cache
        services.AddHybridCache();

        // Validators
        services.AddScoped<TemperatureReadingValidator>();
        services.AddScoped<HeartbeatValidator>();
        services.AddScoped<BatteryChangeValidator>();

        // Application services
        services.AddScoped<TemperatureReadingService>();
        services.AddScoped<HeartbeatService>();
        services.AddScoped<BatteryChangeService>();

        return services;
    }
}

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BeachWeatherStation.Application;
using BeachWeatherStation.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using System.Text.Json;
using BeachWeatherStation.Worker.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureFunctionsWorkerDefaults(builder => {}, options =>
    {
        // User code exception handling is now the default behavior
    })    
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Add MVC services with JSON options for handling HTTP requests
        services.AddMvc();
        
        // Register application services
        services.AddApplicationServices();
        
        // Register infrastructure services (repositories, DB context, etc.)
        services.AddInfrastructure(EnvironmentConfiguration.GetConfiguration());
    })
    .ConfigureLogging(logging =>
    {
        logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            LoggerFilterRule? defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (defaultRule is not null)
            {
                options.Rules.Remove(defaultRule);
            }
        });

        // Configure appropriate logging levels
        logging.AddFilter("Microsoft", LogLevel.Warning);
        logging.AddFilter("System", LogLevel.Warning);
        logging.AddFilter("BeachWeatherStation", LogLevel.Information);
        logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information);
    })    
    .Build();
    
host.Run();
using Microsoft.Extensions.Configuration;

namespace BeachWeatherStation.Worker.Configuration;

public static class EnvironmentConfiguration
{
    public static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}

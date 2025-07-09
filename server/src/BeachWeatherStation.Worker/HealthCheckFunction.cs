using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace BeachWeatherStation.Worker;

public class HealthCheckFunction
{
    private readonly ILogger<HealthCheckFunction> _logger;

    public HealthCheckFunction(ILogger<HealthCheckFunction> logger)
    {
        _logger = logger;
    }

    [Function("HealthCheck")]
    public IActionResult CheckHealth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req)
    {
        _logger.LogInformation("Health check request received");

        try
        {
            // In a real implementation, you would check database connectivity, external services, etc.
            var healthStatus = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Services = new[]
                {
                    new { Name = "Database", Status = "Healthy" },
                    new { Name = "API", Status = "Healthy" }
                }
            };

            _logger.LogInformation("Health check completed successfully");
            return new OkObjectResult(healthStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            var healthStatus = new
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Error = ex.Message
            };
            return new ObjectResult(healthStatus)
            {
                StatusCode = (int)HttpStatusCode.ServiceUnavailable
            };
        }
    }
}

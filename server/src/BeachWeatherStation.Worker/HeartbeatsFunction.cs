using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Application.Services;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace BeachWeatherStation.Worker;

public class HeartbeatsFunction
{
    private readonly ILogger<HeartbeatsFunction> _logger;
    private readonly HeartbeatService _heartbeatService;

    public HeartbeatsFunction(
        ILogger<HeartbeatsFunction> logger,
        HeartbeatService heartbeatService)
    {
        _logger = logger;
        _heartbeatService = heartbeatService;
    }

    [Function("IngestHeartbeat")]
    public async Task<IActionResult> IngestHeartbeat(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v2/heartbeats")] HttpRequest req,
        [FromBody] CreateHeartbeatDto heartbeatDto)
    {
        _logger.LogInformation("Processing heartbeat ingestion request");

        try
        {

            if (heartbeatDto == null)
            {
                _logger.LogWarning("Invalid request format for heartbeat");
                return new BadRequestObjectResult("Invalid request format");
            }

            // Assuming the method exists in HeartbeatService
            var success = await _heartbeatService.IngestHeartbeatAsync(heartbeatDto);
            if (!success)
            {
                _logger.LogWarning("Heartbeat validation failed");
                return new BadRequestObjectResult("Validation failed");
            }

            _logger.LogInformation("Heartbeat ingested successfully");
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing heartbeat ingestion");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("GetLatestHeartbeat")]
    public async Task<IActionResult> GetLatestHeartbeat(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v2/heartbeats/latest/{deviceId?}")] HttpRequest req,
        string deviceId = "Sensor1")
    {
        _logger.LogInformation("Getting latest heartbeat for device {DeviceId}", deviceId);

        try
        {
            // Assuming the method exists in HeartbeatService
            var latestHeartbeat = await _heartbeatService.GetLatestHeartbeatAsync(deviceId);
            
            if (latestHeartbeat == null)
            {
                _logger.LogWarning("No heartbeats found for device {DeviceId}", deviceId);
                return new NotFoundResult();
            }

            _logger.LogInformation("Successfully retrieved latest heartbeat");
            return new OkObjectResult(latestHeartbeat);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving latest heartbeat");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("GetHeartbeatsLast24h")]
    public async Task<IActionResult> GetHeartbeatsLast24h(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v2/heartbeats/last24h/{deviceId?}")] HttpRequest req,
        string deviceId = "Sensor1")
    {
        _logger.LogInformation("Getting heartbeats for last 24h for device {DeviceId}", deviceId);

        try
        {
            // Assuming the method exists in HeartbeatService
            var heartbeats = await _heartbeatService.GetHeartbeatsLast24hAsync(deviceId);
            
            _logger.LogInformation("Successfully retrieved heartbeats for the last 24 hours");
            return new OkObjectResult(heartbeats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving heartbeats for the last 24 hours");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}

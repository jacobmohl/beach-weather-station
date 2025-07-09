using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Application.Services;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace BeachWeatherStation.Worker;

public class BatteryChangesFunction
{
    private readonly ILogger<BatteryChangesFunction> _logger;
    private readonly BatteryChangeService _batteryChangeService;

    public BatteryChangesFunction(
        ILogger<BatteryChangesFunction> logger,
        BatteryChangeService batteryChangeService)
    {
        _logger = logger;
        _batteryChangeService = batteryChangeService;
    }

    [Function("IngestBatteryChange")]
    public async Task<IActionResult> IngestBatteryChange(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "batterychanges")] HttpRequest req,
        [FromBody] CreateBatteryChangeDto batteryChangeDto)
    {
        _logger.LogInformation("Processing battery change ingestion request");

        try
        {
            if (batteryChangeDto == null)
            {
                _logger.LogWarning("Invalid request format for battery change");
                return new BadRequestObjectResult("Invalid request format");
            }

            var success = await _batteryChangeService.IngestBatteryChangeAsync(batteryChangeDto);
            if (!success)
            {
                _logger.LogWarning("Battery change validation failed");
                return new BadRequestObjectResult("Validation failed");
            }

            _logger.LogInformation("Battery change ingested successfully");
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing battery change ingestion");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("GetLatestBatteryChange")]
    public async Task<IActionResult> GetLatestBatteryChange(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "batterychanges/latest/{deviceId}")] HttpRequest req,
        Guid deviceId)
    {
        _logger.LogInformation("Getting latest battery change for device {DeviceId}", deviceId);

        try
        {
            var latestBatteryChange = await _batteryChangeService.GetLatestBatteryChangeAsync(deviceId);
            
            if (latestBatteryChange == null)
            {
                _logger.LogWarning("No battery changes found for device {DeviceId}", deviceId);
                return new NotFoundResult();
            }

            _logger.LogInformation("Successfully retrieved latest battery change");
            return new OkObjectResult(latestBatteryChange);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving latest battery change");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("GetBatteryChangesLast24h")]
    public async Task<IActionResult> GetBatteryChangesLast24h(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "batterychanges/last24h/{deviceId}")] HttpRequest req,
        Guid deviceId)
    {
        _logger.LogInformation("Getting battery changes for last 24h for device {DeviceId}", deviceId);

        try
        {
            var batteryChanges = await _batteryChangeService.GetBatteryChangesLast24hAsync(deviceId);
            
            _logger.LogInformation("Successfully retrieved battery changes for the last 24 hours");
            return new OkObjectResult(batteryChanges);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving battery changes for the last 24 hours");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}

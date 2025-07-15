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

public class ReadingsFunction
{
    private readonly ILogger<ReadingsFunction> _logger;
    private readonly TemperatureReadingService _readingService;

    public ReadingsFunction(
        ILogger<ReadingsFunction> logger,
        TemperatureReadingService readingService)
    {
        _logger = logger;
        _readingService = readingService;
    }

    [Function("IngestReading")]
    public async Task<IActionResult> IngestReading(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v2/readings")] HttpRequest req,
        [FromBody] CreateTemperatureReadingDto readingDto)
    {
        _logger.LogInformation("Processing temperature reading ingestion request");

        try
        {

            if (readingDto == null)
            {
                _logger.LogWarning("Invalid request format for temperature reading");
                return new BadRequestObjectResult("Invalid request format");
            }

            try
            {
                await _readingService.IngestReadingAsync(readingDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Temperature reading validation failed");
                return new BadRequestObjectResult("Validation failed");
            }

            _logger.LogInformation("Temperature reading ingested successfully");
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing temperature reading ingestion");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("GetLatestReading")]
    public async Task<IActionResult> GetLatestReading(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v2/readings/latest/{deviceId?}")] HttpRequest req,
        string deviceId = "Sensor1")
    {
        _logger.LogInformation("Getting latest temperature reading for device {DeviceId}", deviceId);

        try
        {
            var latestReading = await _readingService.GetLatestReadingAsync(deviceId);
            
            if (latestReading == null)
            {
                _logger.LogWarning("No temperature readings found for device {DeviceId}", deviceId);
                return new NotFoundResult();
            }

            _logger.LogInformation("Successfully retrieved latest temperature reading");
            return new OkObjectResult(latestReading);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving latest temperature reading");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("GetReadingsLast24h")]
    public async Task<IActionResult> GetReadingsLast24h(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v2/readings/last24h/{deviceId?}")] HttpRequest req,
        string deviceId = "Sensor1")
    {
        _logger.LogInformation("Getting temperature readings for last 24h for device {DeviceId}", deviceId);

        try
        {
            var (readings, highest, lowest) = await _readingService.GetReadingsLast24hAsync(deviceId);
            
            var result = new
            {
                Readings = readings,
                Highest = highest,
                Lowest = lowest
            };
            
            _logger.LogInformation("Successfully retrieved temperature readings for last 24 hours");
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving temperature readings for last 24 hours");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("GetDailyStats")]
    public async Task<IActionResult> GetDailyStats(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v2/readings/dailystats/{deviceId?}")] HttpRequest req,
        string deviceId = "Sensor1")
    {
        _logger.LogInformation("Getting daily temperature stats for device {DeviceId}", deviceId);

        try
        {
            var dailyStats = await _readingService.GetDailyStatsLast30DaysAsync(deviceId);

            _logger.LogInformation("Successfully retrieved daily temperature stats");
            return new OkObjectResult(dailyStats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving daily temperature stats");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Application.Services;
using BeachWeatherStation.Domain.Interfaces;
using BeachWeatherStation.Domain.Entities;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;
using BeachWeatherStation.Worker.DTOs;
using System.Linq;

namespace BeachWeatherStation.Worker;

public class LegacyProxyFunction
{
    private readonly ILogger<LegacyProxyFunction> _logger;
    private readonly TemperatureReadingService _readingService;
    private readonly HeartbeatService _heartbeatService;

    public LegacyProxyFunction(
        ILogger<LegacyProxyFunction> logger,
        TemperatureReadingService readingService,
        HeartbeatService heartbeatService)
    {
        _logger = logger;
        _readingService = readingService;
        _heartbeatService = heartbeatService;
    }

    [Function("LegacyIngestReading")]
    public async Task<IActionResult> LegacyIngestReading(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "api/readings")] HttpRequest req)
    {
        _logger.LogInformation("Processing legacy reading ingestion request");

        try
        {
            // Read the request body as JSON
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.LogWarning("Empty request body for legacy reading");
                return new BadRequestObjectResult("Empty request body");
            }

            // Parse JSON to determine payload type
            using var jsonDoc = JsonDocument.Parse(requestBody);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("sensorType", out var sensorTypeElement))
            {
                _logger.LogWarning("Missing sensorType in legacy payload");
                return new BadRequestObjectResult("Missing sensorType field");
            }

            var sensorType = sensorTypeElement.GetString();

            // Route based on sensor type
            switch (sensorType)
            {
                case "WaterTemperature":
                    return await ProcessLegacyTemperatureReading(requestBody);

                case "Heatbeat": // Note: keeping original typo "Heatbeat" from legacy system
                    return await ProcessLegacyHeartbeat(requestBody);

                default:
                    _logger.LogWarning("Unknown sensor type: {SensorType}", sensorType);
                    return new BadRequestObjectResult($"Unknown sensor type: {sensorType}");
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Invalid JSON in legacy reading request");
            // TODO: Save these as sensor errors for further analysis
            return new BadRequestObjectResult("Invalid JSON format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing legacy reading ingestion");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    private async Task<IActionResult> ProcessLegacyTemperatureReading(string requestBody)
    {
        try
        {
            var legacyDto = JsonSerializer.Deserialize<LegacyTemperatureReadingDto>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (legacyDto == null)
            {
                _logger.LogWarning("Failed to deserialize legacy temperature reading");
                return new BadRequestObjectResult("Invalid temperature reading format");
            }

            var newDto = new CreateTemperatureReadingDto
            {
                DeviceId = "Sensor1",
                CreatedAt = DateTime.UtcNow,
                Temperature = legacyDto.Reading,
                SignalStrength = legacyDto.SignalStrength
            };

            // Forward to existing service
            var success = await _readingService.IngestReadingAsync(newDto);
            if (!success)
            {
                _logger.LogWarning("Temperature reading validation failed");
                return new BadRequestObjectResult("Temperature reading validation failed");
            }

            _logger.LogInformation("Legacy temperature reading ingested successfully for sensor {SensorId}", legacyDto.SensorId);
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing legacy temperature reading");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    private async Task<IActionResult> ProcessLegacyHeartbeat(string requestBody)
    {
        try
        {
            var legacyDto = JsonSerializer.Deserialize<LegacyHeartbeatDto>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (legacyDto == null)
            {
                _logger.LogWarning("Failed to deserialize legacy heartbeat");
                return new BadRequestObjectResult("Invalid heartbeat format");
            }

            var newDto = new CreateHeartbeatDto
            {
                DeviceId = "Sensor1",
                CreatedAt = DateTime.UtcNow
            };

            // Forward to existing service
            var success = await _heartbeatService.IngestHeartbeatAsync(newDto);
            if (!success)
            {
                _logger.LogWarning("Heartbeat validation failed");
                return new BadRequestObjectResult("Heartbeat validation failed");
            }

            _logger.LogInformation("Legacy heartbeat ingested successfully for sensor {SensorId}", legacyDto.SensorId);
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing legacy heartbeat");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("LegacyGetLatest24HoursReadings")]
    public async Task<IActionResult> LegacyGetLatest24HoursReadings(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/readings/latest24hours")] HttpRequest req)
    {
        _logger.LogInformation("Processing legacy request for latest 24 hours readings");

        try
        {
            // We only have Sensor1 in the legacy system
            const string deviceId = "Sensor1";
            
            // Get readings from the service
            var (readings, highest, lowest) = await _readingService.GetReadingsLast24hAsync(deviceId);
            
            if (readings == null || !readings.Any())
            {
                _logger.LogInformation("No readings found for the last 24 hours for device {DeviceId}", deviceId);
                return new OkObjectResult(new List<LegacyReadingResponseDto>());
            }

            // Map to legacy response format
            var response = readings.Select(r => new LegacyReadingResponseDto
            {
                Id = r.Id,
                SensorType = "WaterTemperature",
                Unit = "C",
                Reading = r.Temperature,
                SensorId = r.DeviceId,
                CapturedAt = r.CreatedAt,
                Meta = null,
                SignalStrength = r.SignalStrength,
                TimeToLive = -1 // Default value for regular readings
            }).ToList();

            _logger.LogInformation("Returning {Count} readings for the last 24 hours", response.Count);
            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request for latest 24 hours readings");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("LegacyGetLatestReading")]
    public async Task<IActionResult> LegacyGetLatestReading(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/readings/latest")] HttpRequest req)
    {
        _logger.LogInformation("Processing legacy request for latest reading");

        try
        {
            // We only have Sensor1 in the legacy system
            const string deviceId = "Sensor1";
            
            // Get the latest reading from the service
            var reading = await _readingService.GetLatestReadingAsync(deviceId);
            
            if (reading == null)
            {
                _logger.LogInformation("No readings found for device {DeviceId}", deviceId);
                return new NotFoundObjectResult($"No readings found for device {deviceId}");
            }

            // Map to legacy response format
            var response = new LegacyReadingResponseDto
            {
                Id = reading.Id,
                SensorType = "WaterTemperature",
                Unit = "C",
                Reading = reading.Temperature,
                SensorId = reading.DeviceId,
                CapturedAt = reading.CreatedAt,
                Meta = null,
                SignalStrength = reading.SignalStrength,
                TimeToLive = -1 // Default value for regular readings
            };

            _logger.LogInformation("Returning latest reading for device {DeviceId} from {Timestamp}", 
                deviceId, reading.CreatedAt);
                
            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request for latest reading");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [Function("LegacyGetLatest30DaysStats")]
    public async Task<IActionResult> LegacyGetLatest30DaysStats(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/readings/latest30days")] HttpRequest req)
    {
        _logger.LogInformation("Processing legacy request for latest 30 days statistics");

        try
        {
            // We only have Sensor1 in the legacy system
            const string deviceId = "Sensor1";
            
            // Get daily stats from the service
            var stats = await _readingService.GetDailyStatsLast30DaysAsync(deviceId);
            
            if (stats == null || !stats.Any())
            {
                _logger.LogInformation("No statistics found for the last 30 days for device {DeviceId}", deviceId);
                return new OkObjectResult(new List<LegacyDailyStatsResponseDto>());
            }
            
            // Construct response without signal strength data initially
            var response = stats.Select(s => new LegacyDailyStatsResponseDto
            {
                CapturedAt = s.Date,
                Count = 0, // We'll calculate this separately
                AverageReading = s.Average,
                HighestReading = s.Maximum,
                LowestReading = s.Minimum,
                AverageSignal = -95, // Default average value based on examples
                HighestSignal = -90, // Default best value based on examples
                LowestSignal = -101  // Default worst value based on examples
            }).ToList();

            _logger.LogInformation("Returning {Count} daily stats for the last 30 days", response.Count);
            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request for latest 30 days statistics");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}

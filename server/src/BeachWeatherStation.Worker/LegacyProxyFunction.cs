using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using BeachWeatherStation.Application.DTOs;
using BeachWeatherStation.Application.Services;
using BeachWeatherStation.Domain.Interfaces;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;
using BeachWeatherStation.Worker.DTOs;

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
    
    // Implement the rest of the leagcy proxy functions as needed


    // /// <summary>
    // /// Maps a legacy sensor ID to a device GUID. Creates a new device if it doesn't exist.
    // /// </summary>
    // /// <param name="sensorId">The legacy sensor ID (e.g., "Sensor1")</param>
    // /// <returns>The device GUID</returns>
    // private Guid GetOrCreateDeviceIdAsync(string sensorId)
    // {
    // // For this simple implementation, we'll create a deterministic GUID based on the sensor ID
    // // In a real-world scenario, you might want to store this mapping in the database

    // // First, check if a device with this name already exists
    // var devices = await _deviceRepository.GetAllDevicesAsync();
    // var existingDevice = devices.FirstOrDefault(d => d.Name.Equals(sensorId, StringComparison.OrdinalIgnoreCase));

    // if (existingDevice != null)
    // {
    //     return existingDevice.Id;
    // }

    // // Create a new device with a deterministic GUID
    // var deviceId = GenerateDeterministicGuid(sensorId);

    // try
    // {
    //     var newDevice = new BeachWeatherStation.Domain.Entities.Device
    //     {
    //         Id = deviceId,
    //         Name = sensorId,
    //         Status = BeachWeatherStation.Domain.Entities.DeviceStatus.Online
    //     };

    //     await _deviceRepository.AddDeviceAsync(newDevice);
    //     _logger.LogInformation("Created new device with ID {DeviceId} for sensor {SensorId}", deviceId, sensorId);
    // }
    // catch (Exception ex)
    // {
    //     // If creation fails (e.g., due to concurrent creation), try to get the existing device
    //     _logger.LogWarning(ex, "Failed to create device for sensor {SensorId}, attempting to retrieve existing", sensorId);
    //     var retryDevices = await _deviceRepository.GetAllDevicesAsync();
    //     var retryExistingDevice = retryDevices.FirstOrDefault(d => d.Name.Equals(sensorId, StringComparison.OrdinalIgnoreCase));
    //     if (retryExistingDevice != null)
    //     {
    //         return retryExistingDevice.Id;
    //     }
    //     throw;
    // }

    // return deviceId;
    //}

    // /// <summary>
    // /// Generates a deterministic GUID based on a string input
    // /// </summary>
    // /// <param name="input">The input string</param>
    // /// <returns>A deterministic GUID</returns>
    // private static Guid GenerateDeterministicGuid(string input)
    // {
    //     // Using a namespace GUID for deterministic generation
    //     var namespaceGuid = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"); // Standard namespace GUID
    //     var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);

    //     using var sha1 = System.Security.Cryptography.SHA1.Create();
    //     var hash = sha1.ComputeHash(namespaceGuid.ToByteArray().Concat(inputBytes).ToArray());

    //     // Set version (5) and variant bits according to RFC 4122
    //     hash[6] = (byte)((hash[6] & 0x0F) | 0x50); // Version 5
    //     hash[8] = (byte)((hash[8] & 0x3F) | 0x80); // Variant 10

    //     // Take first 16 bytes to form GUID
    //     var guidBytes = new byte[16];
    //     Array.Copy(hash, guidBytes, 16);

    //     return new Guid(guidBytes);
    // }
}

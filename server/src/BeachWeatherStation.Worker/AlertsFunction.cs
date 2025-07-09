using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using BeachWeatherStation.Domain.Interfaces;

namespace BeachWeatherStation.Worker;

public class AlertsFunction
{
    private readonly ILogger<AlertsFunction> _logger;
    private readonly ITemperatureReadingRepository _readingRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IAlertService _alertService;

    public AlertsFunction(
        ILogger<AlertsFunction> logger,
        ITemperatureReadingRepository readingRepository,
        IDeviceRepository deviceRepository,
        IAlertService alertService)
    {
        _logger = logger;
        _readingRepository = readingRepository;
        _deviceRepository = deviceRepository;
        _alertService = alertService;
    }

    // [Function("CheckForDelayedReadings")]   
    // public async Task CheckForDelayedReadings([TimerTrigger("0 */5 * * * *", RunOnStartup = false, UseMonitor = true)] TimerInfo timerInfo)
    // {
    //     _logger.LogInformation("Checking for delayed readings at: {Time}", DateTime.UtcNow);

    //     try
    //     {
    //         var devices = await _deviceRepository.GetAllDevicesAsync();
    //         foreach (var device in devices)
    //         {
    //             var latestReading = await _readingRepository.GetLatestReadingAsync(device.Id);
    //             if (latestReading == null)
    //             {
    //                 _logger.LogWarning("No readings found for device {DeviceId}", device.Id);
    //                 await _alertService.NotifyDelayedReading(device.Id, null);
    //                 continue;
    //             }

    //             var timeSinceLastReading = DateTime.UtcNow - latestReading.CreatedAt;
    //             if (timeSinceLastReading > TimeSpan.FromMinutes(30))
    //             {
    //                 _logger.LogWarning("Delayed reading detected for device {DeviceId}. Last reading was {Minutes} minutes ago",
    //                     device.Id, timeSinceLastReading.TotalMinutes);

    //                 await _alertService.NotifyDelayedReading(device.Id, latestReading.CreatedAt);
    //             }
    //         }

    //         _logger.LogInformation("Delayed readings check completed");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error checking for delayed readings");
    //     }
    // }
}

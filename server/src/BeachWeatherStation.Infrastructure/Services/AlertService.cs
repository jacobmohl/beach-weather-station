using BeachWeatherStation.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BeachWeatherStation.Infrastructure.Services;

/// <summary>
/// Service for sending alerts about device issues (e.g., delayed readings).
/// </summary>
public class AlertService : IAlertService
{
    // Logger for alert messages
    private readonly ILogger<AlertService> _logger;
    /// <summary>
    /// Constructor with dependency injection for logger.
    /// </summary>
    public AlertService(ILogger<AlertService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Notify about a delayed reading for a device.
    /// </summary>
    public void NotifyDelayedReading(int deviceId, DateTime lastReadingTime)
    {
        // Example: Log a warning. Replace with email/webhook/Azure Monitor as needed.
        _logger.LogWarning($"Device {deviceId} has a delayed reading. Last reading at {lastReadingTime}.");
    }
}

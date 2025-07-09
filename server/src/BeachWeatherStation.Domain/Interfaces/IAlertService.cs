using System;

namespace BeachWeatherStation.Domain.Interfaces;

/// <summary>
/// Interface for alert services to notify about device issues.
/// </summary>
public interface IAlertService
{
    /// <summary>
    /// Sends an alert when a device hasn't sent readings for an extended period.
    /// </summary>
    /// <param name="deviceId">The ID of the device that has delayed readings.</param>
    /// <param name="lastReadingTime">The timestamp of the last reading received from the device.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task NotifyDelayedReading(Guid deviceId, DateTime? lastReadingTime);
}

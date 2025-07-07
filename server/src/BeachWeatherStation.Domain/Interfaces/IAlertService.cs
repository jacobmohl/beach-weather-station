using System;

namespace BeachWeatherStation.Domain.Interfaces
{
    /// <summary>
    /// Interface for alert services to notify about device issues.
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Notify about a delayed reading for a device.
        /// </summary>
        /// <param name="deviceId">The ID of the device.</param>
        /// <param name="lastReadingTime">The time of the last reading.</param>
        void NotifyDelayedReading(int deviceId, DateTime lastReadingTime);
    }
}

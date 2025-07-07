using System;
using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;
using System.Linq;

namespace BeachWeatherStation.Domain.Services
{
    /// <summary>
    /// Provides functionality to generate alerts based on device readings.
    /// </summary>
    public class AlertService
    {
        /// <summary>
        /// Generates alerts based on the provided readings.
        /// </summary>
        /// <param name="readings">A collection of readings from devices.</param>
        /// <returns>A list of alert messages.</returns>
        public List<string> GenerateAlerts(IEnumerable<Reading> readings)
        {
            var alerts = new List<string>();

            // Get the latest reading based on the timestamp.
            var latestReading = readings.OrderByDescending(r => r.CreatedAt).FirstOrDefault();

            // Check if the latest reading is older than 30 minutes.
            if (latestReading != null && DateTime.UtcNow - latestReading.CreatedAt > TimeSpan.FromMinutes(30))
            {
                alerts.Add($"Latest temperature reading is older than 30 minutes for device {latestReading.DeviceId}.");
            }

            return alerts;
        }
    }
}

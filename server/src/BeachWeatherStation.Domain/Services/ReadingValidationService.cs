using System;
using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Services
{
    /// <summary>
    /// Provides functionality to validate device readings.
    /// </summary>
    public class ReadingValidationService
    {
        /// <summary>
        /// Validates a new reading against existing readings to prevent duplicates.
        /// </summary>
        /// <param name="reading">The new reading to validate.</param>
        /// <param name="existingReadings">A collection of existing readings.</param>
        /// <returns>True if the reading is valid (not a duplicate); otherwise, false.</returns>
        public bool ValidateReading(Reading reading, IEnumerable<Reading> existingReadings)
        {
            foreach (var existingReading in existingReadings)
            {
                // Check for duplicate reading by DeviceId and CreatedAt timestamp.
                if (existingReading.DeviceId == reading.DeviceId && existingReading.CreatedAt == reading.CreatedAt)
                {
                    return false; // Duplicate reading
                }
            }
            return true;
        }
    }
}

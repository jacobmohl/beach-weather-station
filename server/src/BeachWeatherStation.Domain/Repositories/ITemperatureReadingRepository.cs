using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Repositories
{
    /// <summary>
    /// Defines the contract for temperature reading repository operations.
    /// </summary>
    public interface ITemperatureReadingRepository
    {
        /// <summary>
        /// Retrieves a temperature reading by its unique identifier.
        /// </summary>
        /// <param name="temperatuReadingId">The unique identifier of the temperature reading.</param>
        /// <returns>The temperature reading details.</returns>
        TemperatureReading GetTemperatureReadingById(string temperatuReadingId);

        /// <summary>
        /// Retrieves all temperature readings associated with a specific device.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>A collection of temperature readings.</returns>
        IEnumerable<TemperatureReading> GetReadingsByDeviceId(string deviceId);

        /// <summary>
        /// Adds a new temperature reading record.
        /// </summary>
        /// <param name="temperatuReading">The temperature reading details.</param>
        void AddReading(TemperatureReading temperatuReading);

        /// <summary>
        /// Updates an existing temperature reading record.
        /// </summary>
        /// <param name="temperatuReading">The updated temperature reading details.</param>
        void UpdateReading(TemperatureReading temperatuReading);

        /// <summary>
        /// Deletes a temperature reading record by its unique identifier.
        /// </summary>
        /// <param name="temperatuReadingId">The unique identifier of the temperature reading.</param>
        void DeleteReading(string temperatuReadingId);
    }
}

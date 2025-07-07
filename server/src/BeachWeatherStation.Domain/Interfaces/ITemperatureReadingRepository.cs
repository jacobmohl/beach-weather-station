using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for temperature reading repository operations.
    /// </summary>
    public interface ITemperatureReadingRepository
    {
        /// <summary>
        /// Retrieves a temperature reading by its unique identifier.
        /// </summary>
        /// <param name="temperatureReading">The unique identifier of the temperature reading.</param>
        /// <returns>The temperature reading details.</returns>
        TemperatureReading GetTemperatureReadingById(Guid temperatureReadingId);

        /// <summary>
        /// Retrieves all temperature readings associated with a specific device.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>A collection of temperature readings.</returns>
        IEnumerable<TemperatureReading> GetReadingsByDeviceId(Guid deviceId);

        /// <summary>
        /// Adds a new temperature reading record.
        /// </summary>
        /// <param name="temperatureReading">The temperature reading details.</param>
        void AddReading(TemperatureReading temperatureReading);

        /// <summary>
        /// Updates an existing temperature reading record.
        /// </summary>
        /// <param name="temperatureReading">The updated temperature reading details.</param>
        void UpdateReading(TemperatureReading temperatureReading);

        /// <summary>
        /// Deletes a temperature reading record by its unique identifier.
        /// </summary>
        /// <param name="temperatureReadingId">The unique identifier of the temperature reading.</param>
        void DeleteReading(Guid temperatureReadingId);
    }
}

using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Interfaces;

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

    /// <summary>
    /// Gets the latest temperature reading, and the highest and lowest readings in the last 24 hours for a device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A tuple with latest, highest, and lowest readings in the last 24 hours.</returns>
    TemperatureReading? GetLatest(Guid deviceId);

    /// <summary>
    /// Gets all temperature readings for the last 24 hours for a device, including the highest and lowest.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A tuple with all readings, highest, and lowest in the last 24 hours.</returns>
    (IEnumerable<TemperatureReading> Readings, TemperatureReading? Highest, TemperatureReading? Lowest) GetReadingsForLast24hWithMinMax(Guid deviceId);

    /// <summary>
    /// Gets daily average, lowest, and highest temperature readings for the last 30 days for a device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A collection of daily stats (date, avg, min, max).</returns>
    IEnumerable<DailyTemperatureStats> GetDailyStatsForLast30Days(Guid deviceId);
}

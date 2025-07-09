using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Interfaces;

/// <summary>
/// Defines the contract for battery change repository operations.
/// </summary>
public interface IBatteryChangeRepository
{
    /// <summary>
    /// Retrieves a battery change by its unique identifier.
    /// </summary>
    /// <param name="batteryChangeId">The unique identifier of the battery change.</param>
    /// <returns>The battery change details.</returns>
    Task<BatteryChange?> GetBatteryChangeByIdAsync(Guid batteryChangeId);

    /// <summary>
    /// Retrieves all battery changes associated with a specific device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A collection of battery changes.</returns>
    Task<IEnumerable<BatteryChange>> GetBatteryChangesByDeviceIdAsync(Guid deviceId);

    /// <summary>
    /// Adds a new battery change record.
    /// </summary>
    /// <param name="batteryChange">The battery change details.</param>
    Task AddBatteryChangeAsync(BatteryChange batteryChange);

    /// <summary>
    /// Updates an existing battery change record.
    /// </summary>
    /// <param name="batteryChange">The updated battery change details.</param>
    Task UpdateBatteryChangeAsync(BatteryChange batteryChange);

    /// <summary>
    /// Deletes a battery change record by its unique identifier.
    /// </summary>
    /// <param name="batteryChangeId">The unique identifier of the battery change.</param>
    Task DeleteBatteryChangeAsync(Guid batteryChangeId);
    
    /// <summary>
    /// Retrieves the latest battery change for a specific device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>The most recent battery change or null if none exist.</returns>
    Task<BatteryChange?> GetLatestBatteryChangeAsync(Guid deviceId);
    
    /// <summary>
    /// Retrieves battery changes from the last 24 hours for a specific device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A collection of battery changes from the last 24 hours.</returns>
    Task<IEnumerable<BatteryChange>> GetBatteryChangesLast24hAsync(Guid deviceId);
}

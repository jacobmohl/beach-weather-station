using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Interfaces;

/// <summary>
/// Defines the contract for heartbeat repository operations.
/// </summary>
public interface IHeartbeatRepository
{
    /// <summary>
    /// Retrieves a heartbeat by its unique identifier.
    /// </summary>
    /// <param name="heartbeatId">The unique identifier of the heartbeat.</param>
    /// <returns>The heartbeat details.</returns>
    Task<Heartbeat?> GetHeartbeatByIdAsync(Guid heartbeatId);

    /// <summary>
    /// Retrieves all heartbeats associated with a specific device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A collection of heartbeats.</returns>
    Task<IEnumerable<Heartbeat>> GetHeartbeatsByDeviceIdAsync(string deviceId);

    /// <summary>
    /// Adds a new heartbeat record.
    /// </summary>
    /// <param name="heartbeat">The heartbeat details.</param>
    Task AddHeartbeatAsync(Heartbeat heartbeat);

    /// <summary>
    /// Updates an existing heartbeat record.
    /// </summary>
    /// <param name="heartbeat">The updated heartbeat details.</param>
    Task UpdateHeartbeatAsync(Heartbeat heartbeat);

    /// <summary>
    /// Deletes a heartbeat record by its unique identifier.
    /// </summary>
    /// <param name="heartbeatId">The unique identifier of the heartbeat.</param>
    Task DeleteHeartbeatAsync(Guid heartbeatId);
    
    /// <summary>
    /// Retrieves the latest heartbeat for a specific device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>The most recent heartbeat or null if none exist.</returns>
    Task<Heartbeat?> GetLatestHeartbeatAsync(string deviceId);
    
    /// <summary>
    /// Retrieves heartbeats from the last 24 hours for a specific device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A collection of heartbeats from the last 24 hours.</returns>
    Task<IEnumerable<Heartbeat>> GetHeartbeatsLast24hAsync(string deviceId);
}

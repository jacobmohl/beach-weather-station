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
    Heartbeat GetHeartbeatById(Guid heartbeatId);

    /// <summary>
    /// Retrieves all heartbeats associated with a specific device.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    /// <returns>A collection of heartbeats.</returns>
    IEnumerable<Heartbeat> GetHeartbeatsByDeviceId(Guid deviceId);

    /// <summary>
    /// Adds a new heartbeat record.
    /// </summary>
    /// <param name="heartbeat">The heartbeat details.</param>
    void AddHeartbeat(Heartbeat heartbeat);

    /// <summary>
    /// Updates an existing heartbeat record.
    /// </summary>
    /// <param name="heartbeat">The updated heartbeat details.</param>
    void UpdateHeartbeat(Heartbeat heartbeat);

    /// <summary>
    /// Deletes a heartbeat record by its unique identifier.
    /// </summary>
    /// <param name="heartbeatId">The unique identifier of the heartbeat.</param>
    void DeleteHeartbeat(Guid heartbeatId);
}

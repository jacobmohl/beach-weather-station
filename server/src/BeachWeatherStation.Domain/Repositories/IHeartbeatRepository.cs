using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Repositories
{
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
        Heartbeat GetHeartbeatById(string heartbeatId);

        /// <summary>
        /// Retrieves all heartbeats associated with a specific device.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>A collection of heartbeats.</returns>
        IEnumerable<Heartbeat> GetHeartbeatsByDeviceId(string deviceId);

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
        void DeleteHeartbeat(string heartbeatId);
    }
}

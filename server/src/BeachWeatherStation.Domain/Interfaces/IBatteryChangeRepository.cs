using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Interfaces
{
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
        BatteryChange GetBatteryChangeById(Guid batteryChangeId);

        /// <summary>
        /// Retrieves all battery changes associated with a specific device.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>A collection of battery changes.</returns>
        IEnumerable<BatteryChange> GetBatteryChangesByDeviceId(Guid deviceId);

        /// <summary>
        /// Adds a new battery change record.
        /// </summary>
        /// <param name="batteryChange">The battery change details.</param>
        void AddBatteryChange(BatteryChange batteryChange);

        /// <summary>
        /// Updates an existing battery change record.
        /// </summary>
        /// <param name="batteryChange">The updated battery change details.</param>
        void UpdateBatteryChange(BatteryChange batteryChange);

        /// <summary>
        /// Deletes a battery change record by its unique identifier.
        /// </summary>
        /// <param name="batteryChangeId">The unique identifier of the battery change.</param>
        void DeleteBatteryChange(Guid batteryChangeId);
    }
}

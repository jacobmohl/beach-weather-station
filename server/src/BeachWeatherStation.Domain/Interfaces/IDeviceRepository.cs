using System.Collections.Generic;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for device repository operations.
    /// </summary>
    public interface IDeviceRepository
    {
        /// <summary>
        /// Retrieves a device by its unique identifier.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>The device details.</returns>
        Device GetDeviceById(Guid deviceId);

        /// <summary>
        /// Retrieves all devices in the system.
        /// </summary>
        /// <returns>A collection of devices.</returns>
        IEnumerable<Device> GetAllDevices();

        /// <summary>
        /// Adds a new device record.
        /// </summary>
        /// <param name="device">The device details.</param>
        void AddDevice(Device device);

        /// <summary>
        /// Updates an existing device record.
        /// </summary>
        /// <param name="device">The updated device details.</param>
        void UpdateDevice(Device device);

        /// <summary>
        /// Deletes a device record by its unique identifier.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        void DeleteDevice(Guid deviceId);
    }
}

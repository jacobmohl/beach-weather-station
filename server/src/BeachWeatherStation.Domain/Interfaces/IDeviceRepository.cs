using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Interfaces;

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
    Task<Device?> GetDeviceByIdAsync(string deviceId);

    /// <summary>
    /// Retrieves all devices in the system.
    /// </summary>
    /// <returns>A collection of devices.</returns>
    Task<IEnumerable<Device>> GetAllDevicesAsync();

    /// <summary>
    /// Adds a new device record.
    /// </summary>
    /// <param name="device">The device details.</param>
    Task AddDeviceAsync(Device device);

    /// <summary>
    /// Updates an existing device record.
    /// </summary>
    /// <param name="device">The updated device details.</param>
    Task UpdateDeviceAsync(Device device);

    /// <summary>
    /// Deletes a device record by its unique identifier.
    /// </summary>
    /// <param name="deviceId">The unique identifier of the device.</param>
    Task DeleteDeviceAsync(string deviceId);
}

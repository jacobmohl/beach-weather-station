namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents a device in the weather station system.
/// </summary>
public class Device 
{
    /// <summary>
    /// The unique identifier of the device.
    /// </summary>
    public required int DeviceId { get; set; }

    /// <summary>
    /// The name of the device.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The current status of the device.
    /// </summary>
    public required DeviceStatus Status { get; set; }
}

/// <summary>
/// Represents the possible statuses for a device.
/// </summary>
public enum DeviceStatus
{
    Active,
    InActive,
    Maintenance
}

namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents a record of a battery change for a device.
/// </summary>
public class BatteryChange
{
    /// <summary>
    /// The unique identifier of the device.
    /// </summary>
    public required int DeviceId { get; set; }

    /// <summary>
    /// The timestamp when the battery change occurred.
    /// </summary>
    public required DateTime CreatedAt { get; set; }
}

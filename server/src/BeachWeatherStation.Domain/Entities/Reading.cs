namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents a generic reading from a device.
/// </summary>
public abstract class Reading
{
    /// <summary>
    /// The unique identifier of the device.
    /// </summary>
    public required int DeviceId { get; set; }

    /// <summary>
    /// The timestamp when the reading was recorded.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// The signal strength of the reading in dBm, if available.
    /// </summary>
    public int? SignalStrength { get; set; }
}

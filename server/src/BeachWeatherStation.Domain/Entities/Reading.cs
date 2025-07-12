namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents a generic reading from a device.
/// </summary>
public abstract class Reading
{
    /// <summary>
    /// The unique identifier for the entity.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the device.
    /// </summary>
    public required string DeviceId { get; set; }

    /// <summary>
    /// The timestamp when the reading was recorded.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The signal strength of the reading in dBm, if available.
    /// </summary>
    public int? SignalStrength { get; set; }
}

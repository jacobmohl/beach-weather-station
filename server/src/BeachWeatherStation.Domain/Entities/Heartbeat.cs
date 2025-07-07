namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents a heartbeat signal sent by a device.
/// </summary>
public class Heartbeat
{
    /// <summary>
    /// The unique identifier of the device.
    /// </summary>
    public required int DeviceId { get; set; }

    /// <summary>
    /// The timestamp when the heartbeat was sent.
    /// </summary>
    public required DateTime CreatedAt { get; set; }    
}

namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents a heartbeat signal sent by a device.
/// </summary>
public class Heartbeat
{
    /// <summary>
    /// The unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the device.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// The timestamp when the heartbeat was sent.
    /// </summary>
    public DateTime CreatedAt { get; set; }    
}

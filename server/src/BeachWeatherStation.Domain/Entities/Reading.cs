namespace BeachWeatherStation.Domain.Entities;

public abstract class Reading
{
    public required int DeviceId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public int? SignalStrength { get; set; }
}

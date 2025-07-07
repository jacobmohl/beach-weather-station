namespace BeachWeatherStation.Domain.Entities;

public class BatteryChange
{
    public required int DeviceId { get; set; }
    public required DateTime CreatedAt { get; set; }
}

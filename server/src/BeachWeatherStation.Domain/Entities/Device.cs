namespace BeachWeatherStation.Domain.Entities;

public class Device 
{
    public required int DeviceId { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
}

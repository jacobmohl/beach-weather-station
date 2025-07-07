namespace BeachWeatherStation.Application.DTOs;

public class TemperatureReadingDto
{
    public Guid DeviceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public double Temperature { get; set; }
    public int? SignalStrength { get; set; }
}

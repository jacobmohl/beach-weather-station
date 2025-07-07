namespace BeachWeatherStation.Application.DTOs;

public class CreateTemperatureReadingDto
{
    public required Guid DeviceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public double Temperature { get; set; }
    public int SignalStrength { get; set; }
}

namespace BeachWeatherStation.Application.DTOs;

public class CreateTemperatureReadingDto
{
    public required string DeviceId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public required double Temperature { get; set; }
    public int? SignalStrength { get; set; }
}

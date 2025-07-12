namespace BeachWeatherStation.Application.DTOs;

public class CreateBatteryChangeDto
{
    public required string DeviceId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

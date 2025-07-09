namespace BeachWeatherStation.Application.DTOs;

public class CreateBatteryChangeDto
{
    public required string DeviceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

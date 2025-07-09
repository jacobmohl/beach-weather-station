namespace BeachWeatherStation.Application.DTOs;

public class CreateBatteryChangeDto
{
    public required Guid DeviceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

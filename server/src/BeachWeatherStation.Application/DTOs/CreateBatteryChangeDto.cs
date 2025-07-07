namespace BeachWeatherStation.Application.DTOs;

public class CreateBatteryChangeDto
{
    public Guid DeviceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

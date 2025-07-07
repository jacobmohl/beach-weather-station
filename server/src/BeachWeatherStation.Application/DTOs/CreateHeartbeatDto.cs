namespace BeachWeatherStation.Application.DTOs;

public class CreateHeartbeatDto
{
    public Guid DeviceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

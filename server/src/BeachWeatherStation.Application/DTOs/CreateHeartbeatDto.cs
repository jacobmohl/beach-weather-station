namespace BeachWeatherStation.Application.DTOs;

public class CreateHeartbeatDto
{
    public required string DeviceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

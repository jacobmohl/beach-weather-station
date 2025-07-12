namespace BeachWeatherStation.Application.DTOs;

public class CreateHeartbeatDto
{
    public required string DeviceId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

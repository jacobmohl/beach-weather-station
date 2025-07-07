namespace BeachWeatherStation.Application.DTOs;

public class HeartbeatDto
{
    public Guid DeviceId { get; set; }
    public DateTime CreatedAt { get; set; }
}

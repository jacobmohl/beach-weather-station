namespace BeachWeatherStation.Worker.DTOs;

public class LegacyHeartbeatDto
{
    public string SensorType { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Reading { get; set; }
    public string SensorId { get; set; } = string.Empty;
}

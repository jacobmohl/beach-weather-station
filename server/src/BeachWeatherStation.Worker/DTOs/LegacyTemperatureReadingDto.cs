namespace BeachWeatherStation.Worker.DTOs;

public class LegacyTemperatureReadingDto
{
    public string SensorType { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public double Reading { get; set; }
    public string SensorId { get; set; } = string.Empty;
    public int? SignalStrength { get; set; }
}

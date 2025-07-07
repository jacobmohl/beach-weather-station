namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents daily temperature statistics for a device.
/// </summary>
public class DailyTemperatureStats
{
    public required DateTime Date { get; set; }
    public required double Average { get; set; }
    public required double Minimum { get; set; }
    public required double Maximum { get; set; }
}

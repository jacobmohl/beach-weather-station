namespace BeachWeatherStation.Application.DTOs;

public class DailyTemperatureStatsDto
{
    public DateTime Date { get; set; }
    public double Average { get; set; }
    public double Lowest { get; set; }
    public double Highest { get; set; }
}

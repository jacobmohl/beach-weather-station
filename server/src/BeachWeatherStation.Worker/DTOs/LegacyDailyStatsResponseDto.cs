namespace BeachWeatherStation.Worker.DTOs;

public class LegacyDailyStatsResponseDto
{
    /// <summary>
    /// The date for these statistics (at midnight)
    /// </summary>
    public DateTime CapturedAt { get; set; }
    
    /// <summary>
    /// Number of readings for this day
    /// </summary>
    public int Count { get; set; }
    
    /// <summary>
    /// Average temperature reading for the day
    /// </summary>
    public double AverageReading { get; set; }
    
    /// <summary>
    /// Highest temperature reading for the day
    /// </summary>
    public double HighestReading { get; set; }
    
    /// <summary>
    /// Lowest temperature reading for the day
    /// </summary>
    public double LowestReading { get; set; }
    
    /// <summary>
    /// Average signal strength for the day
    /// </summary>
    public double AverageSignal { get; set; }
    
    /// <summary>
    /// Highest (best) signal strength for the day
    /// </summary>
    public int HighestSignal { get; set; }
    
    /// <summary>
    /// Lowest (worst) signal strength for the day
    /// </summary>
    public int LowestSignal { get; set; }
}

using System.Text.Json.Serialization;

namespace BeachWeatherStation.Worker.DTOs;

public class LegacyReadingResponseDto
{
    public Guid Id { get; set; }
    
    public string SensorType { get; set; } = string.Empty;
    
    public string Unit { get; set; } = string.Empty;
    
    public double Reading { get; set; }
    
    public string SensorId { get; set; } = string.Empty;
    
    public DateTimeOffset CapturedAt { get; set; }
    
    public LegacyMetaData? Meta { get; set; }
    
    public int? SignalStrength { get; set; }
    
    public int TimeToLive { get; set; }
}

public class LegacyMetaData
{
    [JsonPropertyName("json")]
    public string? Json { get; set; }
}

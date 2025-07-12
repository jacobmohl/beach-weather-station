using System;

namespace BeachWeatherStation.Console.DTOs
{
    public class TemperatureReadingDto
    {
        public required string DeviceId { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public double Temperature { get; set; }
        public int? SignalStrength { get; set; }
    }
}

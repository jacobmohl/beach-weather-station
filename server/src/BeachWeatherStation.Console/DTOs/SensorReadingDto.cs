using Newtonsoft.Json;
using System;

namespace BeachWeatherStation.Console.DTOs
{
    public class SensorReadingDto
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }

        [JsonProperty("CapturedAt")]
        public DateTimeOffset CapturedAt { get; set; }

        [JsonProperty("Discriminator")]
        public string? Discriminator { get; set; }

        [JsonProperty("Meta")]
        public object? Meta { get; set; }

        [JsonProperty("Reading")]
        public double Reading { get; set; }

        [JsonProperty("SensorId")]
        public string? SensorId { get; set; }

        [JsonProperty("SensorType")]
        public string? SensorType { get; set; }

        [JsonProperty("SignalStrength")]
        public int? SignalStrength { get; set; }

        [JsonProperty("Unit")]
        public string? Unit { get; set; }

        [JsonProperty("id")]
        public string? DocumentId { get; set; }

        [JsonProperty("_rid")]
        public string? ResourceId { get; set; }

        [JsonProperty("_self")]
        public string? SelfLink { get; set; }

        [JsonProperty("_etag")]
        public string? ETag { get; set; }

        [JsonProperty("_attachments")]
        public string? Attachments { get; set; }

        [JsonProperty("_ts")]
        public long Timestamp { get; set; }
    }
}

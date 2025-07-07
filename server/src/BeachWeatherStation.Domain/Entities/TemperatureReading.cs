namespace BeachWeatherStation.Domain.Entities;

/// <summary>
/// Represents a temperature reading from a device.
/// </summary>
public class TemperatureReading : Reading
{
    /// <summary>
    /// The temperature value recorded by the device.
    /// </summary>
    public required double Temperature { get; set; }
}

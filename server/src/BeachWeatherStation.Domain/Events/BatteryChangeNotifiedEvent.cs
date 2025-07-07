using System;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Events;

/// <summary>
/// Represents an event that is triggered when a battery change is notified.
/// </summary>
public class BatteryChangeNotifiedEvent
{
    /// <summary>
    /// The battery change details associated with the event.
    /// </summary>
    public BatteryChange BatteryChange { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BatteryChangeNotifiedEvent"/> class.
    /// </summary>
    /// <param name="batteryChange">The battery change details.</param>
    public BatteryChangeNotifiedEvent(BatteryChange batteryChange)
    {
        BatteryChange = batteryChange;
    }
}

namespace BeachWeatherStation.Domain.Aggregates;

using BeachWeatherStation.Domain.Entities;
using System.Collections.Generic;

public class DeviceAggregate
{
    public required Device Device { get; set; }
    public List<Reading> Readings { get; set; } = new();
    public List<Heartbeat> Heartbeats { get; set; } = new();
    public List<BatteryChange> BatteryChanges { get; set; } = new();

    public void AddReading(Reading reading)
    {
        Readings.Add(reading);
    }

    public void AddHeartbeat(Heartbeat heartbeat)
    {
        Heartbeats.Add(heartbeat);
    }

    public void AddBatteryChange(BatteryChange batteryChange)
    {
        BatteryChanges.Add(batteryChange);
    }
}

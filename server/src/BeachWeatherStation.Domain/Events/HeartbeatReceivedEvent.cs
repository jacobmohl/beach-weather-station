using System;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Events
{
    /// <summary>
    /// Represents an event triggered when a heartbeat is received from a device.
    /// </summary>
    public class HeartbeatReceivedEvent
    {
        /// <summary>
        /// The heartbeat details associated with the event.
        /// </summary>
        public Heartbeat Heartbeat { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatReceivedEvent"/> class.
        /// </summary>
        /// <param name="heartbeat">The heartbeat details.</param>
        public HeartbeatReceivedEvent(Heartbeat heartbeat)
        {
            Heartbeat = heartbeat;
        }
    }
}

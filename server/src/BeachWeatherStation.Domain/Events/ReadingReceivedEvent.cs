using System;
using BeachWeatherStation.Domain.Entities;

namespace BeachWeatherStation.Domain.Events
{
    /// <summary>
    /// Represents an event triggered when a reading is received from a device.
    /// </summary>
    public class ReadingReceivedEvent
    {
        /// <summary>
        /// The reading details associated with the event.
        /// </summary>
        public Reading Reading { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadingReceivedEvent"/> class.
        /// </summary>
        /// <param name="reading">The reading details.</param>
        public ReadingReceivedEvent(Reading reading)
        {
            Reading = reading;
        }
    }
}

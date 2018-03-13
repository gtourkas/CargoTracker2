using System;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class Reading
    {
        public Temp Temperature { get; }

        public DateTime Timestamp { get; }

        public Reading(Temp temperature, DateTime timestamp)
        {
            Temperature = temperature ?? throw new ArgumentNullException(nameof(temperature));
            Timestamp = timestamp;
        }
    }
}

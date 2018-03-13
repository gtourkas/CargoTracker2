using System;

namespace Infra.MongoDB.Monitoring.TempRangeMonitor
{
    public class Reading
    {
        public float Value { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

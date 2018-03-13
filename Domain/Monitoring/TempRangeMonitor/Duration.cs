using System;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class Duration
    {
        public TimeSpan Value { get; set; }

        public static Duration Zero = new Duration(new TimeSpan(0));

        public Duration(TimeSpan value)
        {
            Value = value;
        }

        public void Add(Duration duration)
        {
            Value.Add(duration.Value);
        }

        public bool GreaterThan(Duration duration)
        {
            return this.Value.TotalSeconds > duration.Value.TotalSeconds;
        }
    }
}

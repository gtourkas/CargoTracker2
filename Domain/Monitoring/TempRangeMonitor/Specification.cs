using Domain.Monitoring.TempRangeMonitor;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class Specification
    {
        public Range Range { get; set; }

        public Duration Duration { get; set; }

        public Specification(Range range, Duration duration)
        {
            Range = range;
            Duration = duration;
        }

    }
}
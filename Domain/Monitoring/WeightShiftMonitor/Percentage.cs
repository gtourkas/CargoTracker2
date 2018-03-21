using System;

namespace Domain.Monitoring.WeightShiftMonitor
{
    public class Percentage
    {
        public int Value { get; }

        public Percentage(int value)
        {
            if (value <=0 || value > 100)
                throw new ArgumentException(nameof(value), "should be in (0,100]");

            Value = value;
        }

        public bool GreaterThan(PercentageOffset perc)
        {
            return this.Value > perc.Value;
        }

    }
}

using System;

namespace Domain.Monitoring.WeightShiftMonitor
{
    public class PercentageOffset
    {
        public int Value { get; }

        public PercentageOffset(int value)
        {
            Value = value;
        }

        public bool GreaterThan(PercentageOffset perc)
        {
            return this.Value > perc.Value;
        }

        public bool GreaterThan(Percentage perc)
        {
            return this.Value > perc.Value;
        }

    }
}

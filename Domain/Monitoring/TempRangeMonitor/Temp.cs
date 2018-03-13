using System;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class Temp
    {
        public float Value { get; }

        public Temp(float value)
        {
            if (value < -65 || value >65)
                throw new ArgumentException(nameof(value), $"should be in [-65,65] and is {value}");
            Value = value;
            Value = value;
        }
    }
}

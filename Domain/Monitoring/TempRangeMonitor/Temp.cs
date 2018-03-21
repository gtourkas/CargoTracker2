using System;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class Temp
    {
        public static readonly int MinValue = -65;

        public static readonly int MaxValue = 65;

        public float Value { get; }

        public Temp(float value)
        {
            if (value < MinValue || MaxValue > 65)
                throw new ArgumentException(nameof(value), $"should be in [{MinValue},{MaxValue}] and is {value}");
            Value = value;
            Value = value;
        }
    }
}

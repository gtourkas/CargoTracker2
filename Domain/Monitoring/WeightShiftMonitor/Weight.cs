using System;

namespace Domain.Monitoring.WeightShiftMonitor
{
    public class Weight
    {
        public int Value { get; }

        public Weight(int value)
        {
            if (value <= 0)
                throw new ArgumentException("valid weight is > 0");
            Value = value;
        }

        public PercentageOffset CalcPercDiff(Weight weight)
        {
            var diff = Math.Abs(weight.Value - this.Value)*1.0;
            var percDiff = diff / this.Value;

            return new PercentageOffset( (int)(percDiff * 100)) ;
        }
    }
}
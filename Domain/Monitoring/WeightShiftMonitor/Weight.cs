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

        public Percentage CalcPercDiff(Weight weight)
        {
            return new Percentage(((int)((this.Value - weight.Value)*1.0)/this.Value)*100) ;
        }
    }
}
using System;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class Range
    {
        public Temp From { get;  }

        public Temp Till { get;  }

        public Range(Temp from, Temp till)
        {
            if (from.Value > till.Value)
                throw new InvalidOperationException($"{nameof(from)} needs to be less than {nameof(till)}; currently it is {from.Value} and {till.Value}");

            From = from;
            Till = till;
        }

        public bool IsOut(Temp temp)
        {
            return temp.Value < From.Value || temp.Value > Till.Value;
        }
    }
}

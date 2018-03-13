using Domain.Monitoring.WeightShiftMonitor;
using System;

namespace Domain.Monitoring.WeightShiftMonitor
{
    public class Specification
    {
        public Percentage Percentage { get; set; }

        public Specification(Percentage percentage)
        {
            Percentage = percentage ?? throw new ArgumentNullException(nameof(percentage));
        }

    }
}
using System;
using Domain.Monitoring.Container;

namespace Domain.Monitoring.WeightShiftMonitor.Events
{
    public class AlarmStarted : IEvent
    {
        public ContainerId ContainerId { get; }

        public Reading Reading { get; }

        public PercentageOffset PercOfLargestShift { get; }

        public Directions DirOfLargestShift { get; }

        public AlarmStarted(ContainerId containerId, Reading reading, PercentageOffset percOfLargestShift, Directions dirOfLargestShift)
        {
            ContainerId = containerId ?? throw new ArgumentNullException(nameof(containerId));
            Reading = reading ?? throw new ArgumentNullException(nameof(reading));
            PercOfLargestShift = percOfLargestShift ?? throw new ArgumentNullException(nameof(percOfLargestShift));
            DirOfLargestShift = dirOfLargestShift;
        }
    }

}

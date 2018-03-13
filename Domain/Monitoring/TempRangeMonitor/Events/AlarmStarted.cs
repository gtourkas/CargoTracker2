using System;
using Domain.Monitoring.Container;

namespace Domain.Monitoring.TempRangeMonitor.Events
{
    public class AlarmStarted : IEvent
    {
        public ContainerId ContainerId { get; }

        public Reading Reading { get; }

        public AlarmStarted(ContainerId containerId, Reading reading)
        {
            ContainerId = containerId ?? throw new ArgumentNullException(nameof(containerId)) ;
            Reading = reading ?? throw new ArgumentNullException(nameof(reading));
        }
    }

}

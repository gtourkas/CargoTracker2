using System;
using Domain.Monitoring.Container;

namespace Domain.Monitoring.WeightShiftMonitor.Events
{
    public class AlarmStopped : IEvent
    {
        public ContainerId ContainerId { get; }

        public Reading Reading { get; }

        public AlarmStopped(ContainerId containerId, Reading reading)
        {
            ContainerId = containerId;
            Reading = reading ?? throw new ArgumentNullException(nameof(reading));
        }
    }

}

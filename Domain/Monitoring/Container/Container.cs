using System;
using System.Collections.Generic;
using System.Text;
using Domain.Monitoring.TempRangeMonitor;

namespace Domain.Monitoring.Container
{
    public class Container
    {
        public ContainerId ContainerId { get; private set; }

        public string TrackingId { get; private set; }

        public Specification TempMonitorSpec { get; private set; }

        public Container(ContainerId containerId
            , string trackingId
            , Specification tempMonitorSpec)
        {
            ContainerId = containerId;
            TrackingId = trackingId;
            TempMonitorSpec = tempMonitorSpec;
        }

    }
}

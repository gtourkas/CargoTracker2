using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;


namespace Infra.MongoDB.Monitoring.WeightShiftMonitor
{
    public class Monitor
    {
        [BsonId]
        public string ContainerId { get; set; }

        public int SpecificationPercentage { get; set; }

        public Reading InitialReading { get; set; }

        public bool AlarmStarted { get; set; }
    }
}

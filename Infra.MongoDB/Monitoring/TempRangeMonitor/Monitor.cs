using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Monitoring.TempRangeMonitor
{
    public class Monitor
    {
        [BsonId]
        public string ContainerId { get; set; }

        public float SpecificationRangeFrom { get; set; }

        public float SpecificationRangeTill { get; set; }

        public int SpecificationDuration { get; set; }

        public Reading LastReading { get; set; }

        public Reading[] ConsecOutOfRangeReadings { get; set; }

        public bool AlarmStarted { get; set; }

    }
}

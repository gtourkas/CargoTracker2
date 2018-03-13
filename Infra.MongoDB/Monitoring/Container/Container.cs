using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.MongoDB.Monitoring.Container
{
    public class Container
    {
        [BsonId]
        public string ContainerId { get; set; }

        public string TrackingId { get; set; }

        public int SpecificationRangeFrom { get; set; }

        public int SpecificationRangeTill { get; set; }

        public int Duration { get; set; }

    }
}

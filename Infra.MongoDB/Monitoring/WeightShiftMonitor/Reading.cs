using System;

namespace Infra.MongoDB.Monitoring.WeightShiftMonitor
{
    public class Reading
    {
        public int FrontSide { get; set; }

        public int RightSide { get; set; }

        public int RearSide { get; set; }

        public int LeftSide { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

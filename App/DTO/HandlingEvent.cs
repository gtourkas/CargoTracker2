using System;

namespace App.DTO
{
    public class HandlingEvent
    {
        public int Type { get; set; }

        public string Location { get; set; }

        public string Voyage { get; set; }

        public DateTime Completed { get; set; }

        public DateTime Registered { get; set; }

        public Guid TrackingId { get; set; }

    }
}

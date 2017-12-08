using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.DTO
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

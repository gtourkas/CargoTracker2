using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.DTO
{
    public class Delivery
    {
        public int TransportStatus { get; set; }

        public int RoutingStatus { get; set; }

        public string LastKnownLocation { get; set; }

        public string CurrentVoyage { get; set; }

        public int NextExpectedHandlingType { get; set; }

        public string NextExpectedHandlingLocation { get; set; }

        public string NextExpectedVoyage { get; set; }

        public bool IsUnloadedAtDestination { get; set; }

        public bool IsMishandled { get; set; }
    }
}

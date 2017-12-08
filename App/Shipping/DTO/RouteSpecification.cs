using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.DTO
{
    public class RouteSpecification
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public DateTime ArrivalDeadline { get; set; }

    }
}

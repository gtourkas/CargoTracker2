using System;

namespace App.DTO
{
    public class RouteSpecification
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public DateTime ArrivalDeadline { get; set; }

    }
}

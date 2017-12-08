using Domain.Shipping.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.BookNewCargo
{
    public class Command : ICommand
    {
        public TrackingId TrackingId { get; private set; }
        public RouteSpecification RouteSpec { get; private set; }

        public Command(TrackingId trackingId, RouteSpecification routeSpec)
        {
            if (trackingId == null)
                throw new ArgumentNullException("trackingId");

            if (routeSpec == null)
                throw new ArgumentNullException("routeSpec");

            RouteSpec = routeSpec;
        }
    }
}

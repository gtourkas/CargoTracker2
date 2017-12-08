using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.ChangeCargoRoute
{
    public class Command : ICommand
    {
        public TrackingId TrackingId { get; private set; }

        public RouteSpecification RouteSpecification { get; private set; }

        public Command(TrackingId trackingId, RouteSpecification routeSpec)
        {
            if (trackingId == null)
                throw new ArgumentNullException("trackingId");

            if (routeSpec == null)
                throw new ArgumentNullException("routeSpec");

            TrackingId = trackingId;
            RouteSpecification = routeSpec;
        }

    }
}

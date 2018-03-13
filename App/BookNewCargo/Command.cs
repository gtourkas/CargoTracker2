using System;
using Domain.Shipping.Cargo;

namespace App.BookNewCargo
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

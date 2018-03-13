using System;
using Domain.Shipping.Cargo;

namespace App.ChangeCargoRoute
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

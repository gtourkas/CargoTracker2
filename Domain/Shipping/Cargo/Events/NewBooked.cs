using System;

namespace Domain.Shipping.Cargo.Events
{
    public class NewBooked : IEvent
    {
        public TrackingId TrackingId { get; private set; }
        public RouteSpecification RouteSpec { get; private set; }

        public NewBooked(TrackingId trackingId, RouteSpecification routeSpec)
        {
            if (trackingId == null)
                throw new ArgumentNullException("trackingId");

            if (routeSpec == null)
                throw new ArgumentNullException("routeSpec");

            TrackingId = trackingId;
            RouteSpec = routeSpec;
        }

    }
}

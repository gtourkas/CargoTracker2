using System;

namespace Domain.Shipping.Cargo.Events
{
    public class RouteChanged : IEvent
    {
        public TrackingId TrackingId { get; set; }

        public RouteSpecification RouteSpec { get; set; }


        public RouteChanged(TrackingId trackingId, RouteSpecification routeSpec)
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

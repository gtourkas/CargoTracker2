using System;

namespace Domain.Shipping.Cargo.Events
{
    public class NewBooked : IEvent
    {
        public TrackingId TrackingId { get; private set; }
        public RouteSpecification RouteSpec { get; private set; }

        public NewBooked(TrackingId trackingId, RouteSpecification routeSpec)
        {
            TrackingId = trackingId ?? throw new ArgumentNullException(nameof(trackingId));
            RouteSpec = routeSpec ?? throw new ArgumentNullException(nameof(routeSpec));
        }

    }
}

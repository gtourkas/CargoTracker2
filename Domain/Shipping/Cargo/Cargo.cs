using Domain.Shipping.Cargo.Events;
using System;

namespace Domain.Shipping.Cargo
{
    // Root of the Cargo Aggregate
    public class Cargo : BaseAggregateRoot
    {
        public TrackingId TrackingId { get; private set; }

        public RouteSpecification RouteSpec { get; private set; }

        public Itinerary Itinerary { get; private set; }

        public Delivery Delivery { get; private set; }

        public HandlingEvent LastHandlingEvent { get; private set; }


        public Cargo(TrackingId trackingId, RouteSpecification routeSpec)
        {
            if (trackingId == null)
                throw new ArgumentNullException("trackingId");

            if (routeSpec == null)
                throw new ArgumentNullException("routeSpec");

            TrackingId = trackingId;
            RouteSpec = routeSpec;

            Delivery = new Delivery(RouteSpec, null, null);

            this.Events.Add(new NewBooked(trackingId, routeSpec));
        }

        public void AssignToItinerary(Itinerary itinerary)
        {
            if (itinerary == null)
                throw new ArgumentNullException("itinerary");

            Itinerary = itinerary;

            Delivery = new Delivery(RouteSpec, Itinerary, LastHandlingEvent);

            this.Events.Add(new AssignedToItinerary(TrackingId, Itinerary));
            this.Events.Add(new DeliveryStateChanged(TrackingId, Delivery));
        }

        public void ChangeRoute(RouteSpecification routeSpec)
        {
            if (routeSpec == null)
                throw new ArgumentNullException("routeSpec");

            RouteSpec = routeSpec;

            Delivery = new Delivery(RouteSpec, Itinerary, LastHandlingEvent);

            this.Events.Add(new RouteChanged(TrackingId, RouteSpec));
            this.Events.Add(new DeliveryStateChanged(TrackingId, Delivery));
        }

        public void RegisterHandlingEvent(HandlingEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("event");

            LastHandlingEvent = @event;

            Delivery = new Delivery(RouteSpec, Itinerary, LastHandlingEvent);

            this.Events.Add(new HandlingEventRegistered(@event));
            this.Events.Add(new DeliveryStateChanged(TrackingId, Delivery));
        }

    }
}

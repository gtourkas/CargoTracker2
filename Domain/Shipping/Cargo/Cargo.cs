using System;
using Domain.Shipping.Cargo.Events;

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
            TrackingId = trackingId ?? throw new ArgumentNullException(nameof(trackingId));
            RouteSpec = routeSpec ?? throw new ArgumentNullException(nameof(routeSpec));

            Delivery = new Delivery(RouteSpec, null, null);

            this.Events.Add(new NewBooked(trackingId, routeSpec));
        }

        // rehydration ctor
        public Cargo(TrackingId trackingId, RouteSpecification routeSpec, Itinerary itinerary, Delivery delivery, HandlingEvent lastHandlingEvent)
        {
            TrackingId = trackingId;
            RouteSpec = routeSpec;
            Itinerary = itinerary;
            LastHandlingEvent = lastHandlingEvent;

            Delivery = new Delivery(RouteSpec, Itinerary, LastHandlingEvent);
        }

        public void AssignToItinerary(Itinerary itinerary)
        {
            Itinerary = itinerary ?? throw new ArgumentNullException(nameof(itinerary));

            Delivery = new Delivery(RouteSpec, Itinerary, LastHandlingEvent);

            this.Events.Add(new AssignedToItinerary(TrackingId, Itinerary));
            this.Events.Add(new DeliveryStateChanged(TrackingId, Delivery));
        }

        public void ChangeRoute(RouteSpecification routeSpec)
        {
            RouteSpec = routeSpec ?? throw new ArgumentNullException(nameof(routeSpec));

            Delivery = new Delivery(RouteSpec, Itinerary, LastHandlingEvent);

            this.Events.Add(new RouteChanged(TrackingId, RouteSpec));
            this.Events.Add(new DeliveryStateChanged(TrackingId, Delivery));
        }

        public void RegisterHandlingEvent(HandlingEvent @event)
        {
            LastHandlingEvent = @event ?? throw new ArgumentNullException(nameof(@event));

            Delivery = new Delivery(RouteSpec, Itinerary, LastHandlingEvent);

            this.Events.Add(new HandlingEventRegistered(@event));
            this.Events.Add(new DeliveryStateChanged(TrackingId, Delivery));
        }

    }
}

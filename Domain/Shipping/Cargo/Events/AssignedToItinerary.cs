using System;

namespace Domain.Shipping.Cargo.Events
{
    public class AssignedToItinerary : IEvent
    {
        public TrackingId TrackingId { get; private set; }

        public Itinerary Itinerary { get; private set; }

        public AssignedToItinerary(TrackingId trackingId, Itinerary itinerary)
        {
            if (trackingId == null)
                throw new ArgumentNullException("trackingId");

            if (itinerary == null)
                throw new ArgumentNullException("itinerary");

            TrackingId = trackingId;
            Itinerary = itinerary;
        }

    }
}

using System;
using Domain.Shipping.Cargo;

namespace App.AssignCargoToItinerary
{
    public class Command : ICommand
    {
        public TrackingId TrackingId { get; private set; }

        public Itinerary Itinerary { get; private set; }

        public Command(TrackingId trackingId, Itinerary itinerary)
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

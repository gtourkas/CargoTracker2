using Domain.Shipping.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.AssignCargoToItinerary
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

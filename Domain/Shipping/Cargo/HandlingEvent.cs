using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using System;

namespace Domain.Shipping.Cargo
{

    public class HandlingEvent 
    {
        public TrackingId TrackingId { get; private set; }

        public HandlingType Type { get; private set; }

        public UnLocode Location { get; private set; }

        public VoyageNumber Voyage { get; private set; }

        public DateTime Completed { get; private set; }

        public DateTime Registered { get; private set; }

        public HandlingEvent(
            TrackingId trackingId
            , HandlingType type
            , UnLocode location
            , VoyageNumber voyage
            , DateTime completed
            , DateTime registered)
        {
            if (trackingId == null)
                throw new ArgumentNullException("trackingId");

            if (location == null)
                throw new ArgumentNullException("location");

            if ((type == HandlingType.Load || type == HandlingType.Unload)
                &&
                (voyage == null)
               )
                throw new InvalidOperationException("loading/unloading events need a voyage");

            TrackingId = trackingId;
            Type = type;
            Location = location;
            Voyage = voyage;
            Completed = completed;
            Registered = registered;
        }

    }
}

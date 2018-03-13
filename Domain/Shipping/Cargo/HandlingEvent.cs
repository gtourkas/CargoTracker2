using System;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;

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
            if ((type == HandlingType.Load || type == HandlingType.Unload)
                &&
                (voyage == null)
               )
                throw new InvalidOperationException("loading/unloading events need a voyage");

            TrackingId = trackingId ?? throw new ArgumentNullException(nameof(trackingId));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            Type = type;
            Voyage = voyage;
            Completed = completed;
            Registered = registered;
        }

    }
}

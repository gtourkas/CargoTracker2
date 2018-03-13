using System;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;

namespace Domain.Shipping.Cargo
{
    public class HandlingActivity
    {
        public HandlingType Type { get; private set; }

        public UnLocode Location { get; private set; }

        public VoyageNumber Voyage { get; private set; }

        public HandlingActivity(HandlingType type
            , UnLocode location
            , VoyageNumber voyage)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));

            if (type == HandlingType.Load && voyage == null)
                throw new InvalidOperationException("a load activity needs a voyage");

            if (type == HandlingType.Unload && voyage == null)
                throw new InvalidOperationException("an unload activity needs a voyage");

            Type = type;
            Voyage = voyage;
        }
    }
}

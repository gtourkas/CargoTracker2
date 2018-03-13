using System;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;

namespace Domain.Shipping.Cargo
{
    public class Leg
    {
        public VoyageNumber Voyage { get; private set; }

        public UnLocode LoadLocation { get; private set; }

        public UnLocode UnloadLocation { get; private set; }

        public DateTime LoadTime { get; private set; }

        public DateTime UnloadTime { get; private set; }

        public Leg(VoyageNumber voyage,
            UnLocode loadLocation,
            UnLocode unloadLocation,
            DateTime loadTime,
            DateTime unloadTime)
        {
            Voyage = voyage ?? throw new ArgumentNullException(nameof(voyage));

            LoadLocation = loadLocation ?? throw new ArgumentNullException(nameof(loadLocation));

            UnloadLocation = unloadLocation ?? throw new ArgumentNullException(nameof(unloadLocation));

            if (loadTime >= unloadTime)
                throw new ArgumentException("unloadTime should be later than loadTime");

            LoadTime = loadTime;
            UnloadTime = unloadTime;
        }

    }
}

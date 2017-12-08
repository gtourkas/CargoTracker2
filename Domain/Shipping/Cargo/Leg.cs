using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using System;

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

            if (voyage == null)
                throw new ArgumentNullException("voyage");

            if (loadLocation == null)
                throw new ArgumentNullException("loadLocation");

            if (unloadLocation == null)
                throw new ArgumentNullException("unloadLocation");

            if (loadTime >= unloadTime)
                throw new ArgumentException("unloadTime should be later than loadTime");

            Voyage = voyage;
            LoadLocation = loadLocation;
            UnloadLocation = unloadLocation;
            LoadTime = loadTime;
            UnloadTime = unloadTime;
        }

    }
}

using System;
using Domain.Shipping.Location;

namespace Domain.Shipping.Voyage
{
    public class CarrierMovement
    {
        public UnLocode DepartureLocation { get; private set; }

        public UnLocode ArrivalLocation { get; private set; }

        public DateTime DepartureTime { get; private set; }

        public DateTime ArrivalTime { get; private set; }

        public CarrierMovement(
            UnLocode departureLocation,
            UnLocode arrivalLocation,
            DateTime departureTime,
            DateTime arrivalTime)
        {
            // TODO: validation

            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }

    }
}

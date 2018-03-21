using System;
using Domain.Shipping.Location;

namespace Domain.Shipping.Cargo
{
    public class RouteSpecification 
    {
        public UnLocode Origin { get; private set; }

        public UnLocode Destination { get; private set; }

        public DateTime ArrivalDeadline { get; private set; }

        public RouteSpecification(UnLocode origin, UnLocode destination, DateTime arrivalDeadline)
        {

            Origin = origin ?? throw new ArgumentNullException(nameof(origin));

            Destination = destination ?? throw new ArgumentNullException(nameof(destination));

            if (origin.Equals(destination))
                throw new InvalidOperationException("Provided origin and destination are the same");

            ArrivalDeadline = arrivalDeadline;
        }

        public bool IsSatisfiedBy(Itinerary itinerary)
        {
            return itinerary.FirstLoadLocation.Equals(Origin)
                &&
                itinerary.LastUnloadLocation.Equals(Destination)
                &&
                itinerary.FinalArrivalDate <= ArrivalDeadline;
        }

    }
}

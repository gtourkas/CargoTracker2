using Domain.Shipping.Location;
using System;

namespace Domain.Shipping.Cargo
{
    // Value Object
    public interface IRouteSpecification
    {
        bool IsSatisfiedBy(IItinerary itinerary);
        UnLocode Destination { get; }
    }

    public class RouteSpecification : IRouteSpecification
    {
        public UnLocode Origin { get; private set; }

        public UnLocode Destination { get; private set; }

        public DateTime ArrivalDeadline { get; private set; }

        public RouteSpecification(UnLocode origin, UnLocode destination, DateTime arrivalDeadline)
        {
            if (origin == null)
                throw new ArgumentNullException(nameof(origin));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (origin.Equals(destination))
                throw new InvalidOperationException("Provided origin and destination are the same");

            Origin = origin;
            Destination = destination;
            ArrivalDeadline = arrivalDeadline;
        }

        public bool IsSatisfiedBy(IItinerary itinerary)
        {
            return itinerary.FirstLoadLocation.Equals(Origin)
                &&
                itinerary.LastUnloadLocation.Equals(Destination)
                &&
                itinerary.FinalArrivalDate <= ArrivalDeadline;
        }

    }
}

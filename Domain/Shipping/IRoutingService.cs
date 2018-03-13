using System.Collections.Generic;
using Domain.Shipping.Cargo;

namespace Domain.Shipping
{
    public interface IRoutingService
    {
        IList<Itinerary> GetPossibleItineraries(RouteSpecification routeSpec);
    }
}

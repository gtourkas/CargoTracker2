using Domain.Shipping.Cargo;
using System.Collections.Generic;

namespace Domain.Shipping
{
    public interface IRoutingService
    {
        IList<Itinerary> GetPossibleItineraries(RouteSpecification routeSpec);
    }
}

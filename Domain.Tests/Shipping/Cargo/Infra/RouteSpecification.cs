using AutoFixture;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Domain.Shipping.Cargo;
using AutoFixture.Xunit2;

namespace Domain.Tests.Shipping.Cargo.Infra
{
    public class RouteSpecificationBuilder : ISpecimenBuilder
    {
        private DateTime _arrivalDeadline;

        public RouteSpecificationBuilder(DateTime arrivalDeadline)
        {
            _arrivalDeadline = arrivalDeadline;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var paramInfo = request as ParameterInfo;
            if (paramInfo != null
                && paramInfo.Name == "arrivalDeadline")
            {
                return _arrivalDeadline;
            }

            return new NoSpecimen();
        }

    }

    public class RouteSpecAndSatisfyingItineraryCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // RouteSpecification's ArrivalDeadline is later than any* Itinerary's Last Unloading Time
            fixture.Customizations.Add(new RouteSpecificationBuilder(DateTime.MaxValue));
            var routeSpec = fixture.Create<RouteSpecification>();
            fixture.Inject(routeSpec);

            fixture.Customize(new DefaultLegCustomization());

            // RouteSpecification's Origin given is the same as Itinerary's First Loading Location
            // RouteSpecification's Destination given is the same as Itinerary's Last UnLoading Location
            fixture.Customizations.Add(new LegCollectionBuilder(new[] { routeSpec.Origin, null, null, routeSpec.Destination }));
            var legs = new List<Leg>
            {
                fixture.Create<Leg>(),
                fixture.Create<Leg>()
            };

            fixture.Customizations.Add(new ItineraryBuilder(legs));
            fixture.Inject(fixture.Create<Itinerary>());
        }
    }

}

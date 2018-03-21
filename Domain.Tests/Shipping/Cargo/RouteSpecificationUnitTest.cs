using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Tests.Shipping.Cargo.Infra;
using Moq;
using Xunit;
using RouteSpecification = Domain.Shipping.Cargo.RouteSpecification;

namespace Domain.Tests.Shipping.Cargo
{
    public class RouteSpecificationUnitTest
    {
        [Theory]
        [AutoData]
        public void Ctor__NoOriginGiven__ThrowsArgumentNullException(
                UnLocode location
            )
        {
            Assert.Throws<ArgumentNullException>(() => new RouteSpecification(null, location, DateTime.UtcNow));
        }

        [Theory]
        [AutoData]
        public void Ctor__NoDestinationGiven__ThrowsArgumentNullException(
                UnLocode location
            )
        {
            Assert.Throws<ArgumentNullException>(() => new RouteSpecification(location, null, DateTime.UtcNow));
        }

        [Theory]
        [AutoData]
        public void Ctor__SameOriginAndDestinationGiven__ThrowsInvalidOperationException(
                UnLocode location
            )
        {
            Assert.Throws<InvalidOperationException>(() => new RouteSpecification(location, location, DateTime.UtcNow));
        }

        [Theory]
        [AutoData]
        public void IsSatisfiedBy__OriginGivenIsDifferentThanItineraryFirstLoadingLocation__ReturnsFalse(
            RouteSpecification sut
            )
        {
            // ARRANGE
            // NOTE: relying on A/F to create different locations as the RouteSpecification's Origin and the Itinerary's First Loading Location
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var r = sut.IsSatisfiedBy(itinerary);

            // ASSERT
            Assert.False(r);
        }

        [Theory]
        [AutoData]
        public void IsSatisfiedBy__DestinationGivenIsDifferentThanItineraryLastUnLoadingLocation__ReturnFalse(
            RouteSpecification sut
            )
        {
            // ARRANGE
            var itineraryFixture = new Fixture();
            itineraryFixture.Customize(new DefaultLegCustomization());
            // RouteSpecification's Origin given is the same as Itinerary's First Loading Location
            itineraryFixture.Customizations.Add(new LegCollectionBuilder( new [] { sut.Origin } ) );
            var itinerary = itineraryFixture.Create<Itinerary>();

            // ACT
            var r = sut.IsSatisfiedBy(itinerary);

            // ASSERT
            Assert.False(r);
        }

        [Fact]
        public void IsSatisfiedBy__FinalArrivalDateGivenLaterThanArrivalDeadline__ReturnFalse()
        {
            // ARRANGE
            var sutFixture = new Fixture();
            // RouteSpecification's ArrivalDeadline is earlier than any* Itinerary's Last Unloading Time
            sutFixture.Customizations.Add(new RouteSpecificationBuilder(DateTime.MinValue));
            var sut = sutFixture.Create<RouteSpecification>();

            var itineraryFixture = new Fixture();
            itineraryFixture.Customize(new DefaultLegCustomization());

            // RouteSpecification's Origin given is the same as Itinerary's First Loading Location
            // RouteSpecification's Destination given is the same as Itinerary's Last UnLoading Location
            itineraryFixture.Customizations.Add(new LegCollectionBuilder(new[] { sut.Origin, null, null, sut.Destination }));
            var legs = new List<Leg>
            {
                itineraryFixture.Create<Leg>(),
                itineraryFixture.Create<Leg>()
            };

            itineraryFixture.Customizations.Add(new ItineraryBuilder(legs));
            var itinerary = itineraryFixture.Create<Itinerary>();

            // ACT
            var r = sut.IsSatisfiedBy(itinerary);

            // ASSERT
            Assert.False(r);
        }


        [Fact]
        public void IsSatisfiedBy__ReturnsTrue()
        {
            var fixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var sut = fixture.Create<RouteSpecification>();
            var itinerary = fixture.Create<Itinerary>();

            // ACT
            var r = sut.IsSatisfiedBy(itinerary);

            // ASSERT
            Assert.True(r);
        }
    }
}

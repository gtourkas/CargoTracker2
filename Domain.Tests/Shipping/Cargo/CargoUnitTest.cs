using System;
using AutoFixture.Xunit2;
using Domain.Shipping.Cargo;
using Domain.Shipping.Cargo.Events;
using Domain.Tests.Shipping.Cargo.Infra;
using FluentAssertions;
using Xunit;
using AutoFixture;

namespace Domain.Tests.Shipping.Cargo
{
    public class CargoUnitTest
    {
        [Theory]
        [AutoData]
        public void Ctor__NoTrackingIdGiven__ThrowsArgumentNullException(
            RouteSpecification routeSpec
            )
        {
            Assert.Throws<ArgumentNullException>(() => new Domain.Shipping.Cargo.Cargo(null, routeSpec));
        }

        [Theory]
        [AutoData]
        public void Ctor__NoRouteSpecGiven__ThrowsArgumentNullException(
            TrackingId trackingId
        )
        {
            Assert.Throws<ArgumentNullException>(() => new Domain.Shipping.Cargo.Cargo(trackingId, null));
        }

        [Theory]
        [AutoData]
        public void Ctor__EmitsNewBookEvent(
            TrackingId trackingId,
            RouteSpecification routeSpec
        )
        {
            // ACT
            var sut = new Domain.Shipping.Cargo.Cargo(trackingId, routeSpec);

            // ASSERT
            Assert.Equal(routeSpec, sut.RouteSpec);
            Assert.Equal(routeSpec, sut.Delivery.RouteSpec);

            sut.Events[0].Should().BeEquivalentTo(new NewBooked(trackingId, routeSpec));
        }   

        [Theory]
        [AutoData]
        public void AssignToItinerary__NoItineraryGiven__ThrowsArgumentNullException(
            Domain.Shipping.Cargo.Cargo sut
        )
        {
            Assert.Throws<ArgumentNullException>(() => sut.AssignToItinerary(null));
        }

        [Theory]
        [AutoData]
        public void AssignToItinerary__EmitsAssignedToItineraryEvent_and_EmitsDeliveryStateChanged(
            Domain.Shipping.Cargo.Cargo sut
        )
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            sut.AssignToItinerary(itinerary);

            // ASSERT
            Assert.Equal(itinerary, sut.Itinerary);
            Assert.Equal(itinerary, sut.Delivery.Itinerary);

            sut.Events[1].Should().BeEquivalentTo(new AssignedToItinerary(sut.TrackingId, itinerary));
            sut.Events[2].Should().BeEquivalentTo(new DeliveryStateChanged(sut.TrackingId, sut.Delivery));
        }

        [Theory]
        [AutoData]
        public void ChangeRoute__NoRouteSpecGiven__ThrowsArgumentNullException(
                Domain.Shipping.Cargo.Cargo sut
            )
        {
            Assert.Throws<ArgumentNullException>(() => sut.ChangeRoute(null));
        }

        [Theory]
        [AutoData]
        public void ChangeRoute__EmitsAssignedToItineraryEvent_and_EmitsDeliveryStateChangedEvent(
            Domain.Shipping.Cargo.Cargo sut,
            RouteSpecification routeSpec
        )
        {
            // ACT
            sut.ChangeRoute(routeSpec);

            // ASSERT
            Assert.Equal(routeSpec, sut.RouteSpec);
            Assert.Equal(routeSpec, sut.Delivery.RouteSpec);

            sut.Events[1].Should().BeEquivalentTo(new RouteChanged(sut.TrackingId, routeSpec));
            sut.Events[2].Should().BeEquivalentTo(new DeliveryStateChanged(sut.TrackingId, sut.Delivery));
        }

        [Theory]
        [AutoData]
        public void RegisterHandlingEvent__EmitsHandlingEventRegisteredEvent_and_EmitsDeliveryStateChangedEvent(
            Domain.Shipping.Cargo.Cargo sut,
            HandlingEvent @event
        )
        {
            // ACT
            sut.RegisterHandlingEvent(@event);

            // ASSERT
            Assert.Equal(@event, sut.LastHandlingEvent);
            Assert.Equal(@event, sut.Delivery.LastHandlingEvent);

            sut.Events[1].Should().BeEquivalentTo(new HandlingEventRegistered(@event));
            sut.Events[2].Should().BeEquivalentTo(new DeliveryStateChanged(sut.TrackingId, sut.Delivery));
        }

    }
}

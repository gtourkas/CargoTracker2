using Domain.Shipping;
using Domain.Shipping.Cargo;
using Domain.Shipping.Cargo.Events;
using Ploeh.AutoFixture.Xunit2;
using SemanticComparison.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests.Shipping.Cargo
{
    public class CargoUnitTest
    {
        [Theory]
        [AutoData]
        public void Ctor_NoTrackingId_ArgumentNullException(
            RouteSpecification routeSpec
            )
        {
            Assert.Throws<ArgumentNullException>(() => new Domain.Shipping.Cargo.Cargo(null, routeSpec));
        }

        [Theory]
        [AutoData]
        public void Ctor_NoRouteSpec_ArgumentNullException(
            TrackingId trackingId
        )
        {
            Assert.Throws<ArgumentNullException>(() => new Domain.Shipping.Cargo.Cargo(trackingId, null));
        }

        [Theory]
        [AutoData]
        public void Ctor_EmitsNewBookEvent(
            TrackingId trackingId,
            RouteSpecification routeSpec
        )
        {
            // ACT
            var sut = new Domain.Shipping.Cargo.Cargo(trackingId, routeSpec);

            // ASSERT
            Assert.Equal(routeSpec, sut.RouteSpec);
            Assert.Equal(routeSpec, sut.Delivery.RouteSpec);

            var expNewBooked = new NewBooked(trackingId, routeSpec)
                    .AsSource().OfLikeness<NewBooked>();
            Assert.True(expNewBooked.Equals(sut.Events[0]));
        }

        [Theory]
        [AutoData]
        public void AssignToItinerary_NoItinerary_ThrowsArgumentNullException(
            Domain.Shipping.Cargo.Cargo sut
        )
        {
            Assert.Throws<ArgumentNullException>(() => sut.AssignToItinerary(null));
        }

        [Theory]
        [AutoCargoData]
        public void AssignToItinerary_EmitsAssignedToItineraryAndDeliveryStateChanged(
            Domain.Shipping.Cargo.Cargo sut,
            Itinerary itinerary
        )
        {
            // ACT
            sut.AssignToItinerary(itinerary);

            // ASSERT
            Assert.Equal(itinerary, sut.Itinerary);
            Assert.Equal(itinerary, sut.Delivery.Itinerary);

            var expAssignedToItinerary = new AssignedToItinerary(sut.TrackingId, itinerary)
                .AsSource().OfLikeness<AssignedToItinerary>();
            Assert.True(expAssignedToItinerary.Equals(sut.Events[1]));
            var expDeliveryStateChanged = new DeliveryStateChanged(sut.TrackingId, sut.Delivery)
                .AsSource().OfLikeness<DeliveryStateChanged>();
            Assert.True(expDeliveryStateChanged.Equals(sut.Events[2]));
        }

        [Theory]
        [AutoData]
        public void ChangeRoute_NoRouteSpec_ThrowsArgumentNullException(
                Domain.Shipping.Cargo.Cargo sut
            )
        {
            Assert.Throws<ArgumentNullException>(() => sut.ChangeRoute(null));
        }

        [Theory]
        [AutoCargoData]
        public void ChangeRoute_EmitsAssignedToItineraryAndDeliveryStateChanged(
            Domain.Shipping.Cargo.Cargo sut,
            RouteSpecification routeSpec
        )
        {
            // ACT
            sut.ChangeRoute(routeSpec);

            // ASSERT
            Assert.Equal(routeSpec, sut.RouteSpec);
            Assert.Equal(routeSpec, sut.Delivery.RouteSpec);

            var expRouteChanged = new RouteChanged(sut.TrackingId, routeSpec)
                .AsSource().OfLikeness<RouteChanged>();
            Assert.True(expRouteChanged.Equals(sut.Events[1]));
            var expDeliveryStateChanged = new DeliveryStateChanged(sut.TrackingId, sut.Delivery)
                .AsSource().OfLikeness<DeliveryStateChanged>();
            Assert.True(expDeliveryStateChanged.Equals(sut.Events[2]));
        }

        [Theory]
        [AutoCargoData]
        public void RegisterHandlingEvent_EmitsHandlingEventRegisteredAndDeliveryStateChanged(
            Domain.Shipping.Cargo.Cargo sut,
            HandlingEvent @event
        )
        {
            // ACT
            sut.RegisterHandlingEvent(@event);

            // ASSERT
            Assert.Equal(@event, sut.LastHandlingEvent);
            Assert.Equal(@event, sut.Delivery.LastHandlingEvent);

            var expHandlingEventRegistered = new HandlingEventRegistered(@event)
                .AsSource().OfLikeness<HandlingEventRegistered>();
            Assert.True(expHandlingEventRegistered.Equals(sut.Events[1]));
            var expDeliveryStateChanged = new DeliveryStateChanged(sut.TrackingId, sut.Delivery)
                .AsSource().OfLikeness<DeliveryStateChanged>();
            Assert.True(expDeliveryStateChanged.Equals(sut.Events[2]));
        }

    }
}

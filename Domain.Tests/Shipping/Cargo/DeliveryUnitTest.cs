using System;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Domain.Shipping;
using Ploeh.AutoFixture;
using System.Collections.Generic;

namespace Domain.Tests.Shipping.Cargo
{
    public class DeliveryUnitTest   
    {
        [Fact]
        public void Ctor_NoRoutSpec_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Delivery(null, null, null));
        }

        #region Transport Status 

        [Theory]
        [AutoCargoData]
        public void Ctor_NoEvent_TransportStatusNoReceived(
            RouteSpecification routeSpec
            , Itinerary itinerary
            , HandlingEvent @event
        )
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(TransportStatus.NotReceived, sut.TransportStatus);
        }

        [Theory]
        [InlineAutoCargoData(HandlingType.Load, TransportStatus.OnBoardVessel)]
        [InlineAutoCargoData(HandlingType.Unload, TransportStatus.InPort)]
        [InlineAutoCargoData(HandlingType.Receive, TransportStatus.InPort)]
        [InlineAutoCargoData(HandlingType.Customs, TransportStatus.InPort)]
        [InlineAutoCargoData(HandlingType.Claim, TransportStatus.Claimed)]
        [InlineAutoCargoData(HandlingType.Unload, TransportStatus.InPort)]
        public void Ctor_HandlingTypeToTransportStatus(
            HandlingType type
            , TransportStatus status
            , RouteSpecification routeSpec
            , Itinerary itinerary
            , HandlingEvent @event)
        {
            // ARRANGE
            @event = @event.RecreateWith(type);

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(status, sut.TransportStatus);
        }

        #endregion

        #region Routing Status 

        [Theory]
        [AutoCargoData]
        public void Ctor_NoItinerary_NotRouted(
            RouteSpecification routeSpec
            , Itinerary itinerary)
        {
            // ACT
            var sut = new Delivery(routeSpec, null, null);

            // ASSERT
            Assert.Equal(RoutingStatus.NotRouted, sut.RoutingStatus);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_RouteSpecSatifiedByItinerary_Routed(
            Mock<IRouteSpecification> routeSpec
            , Itinerary itinerary)
        {
            // ARRANGE
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(true);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary, null);

            // ASSERT
            Assert.Equal(RoutingStatus.Routed, sut.RoutingStatus);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_RouteSpecNotSatifiedByItinerary_MisRouted(
            Mock<IRouteSpecification> routeSpec
            , Itinerary itinerary)
        {
            // ARRANGE
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(false);

            // ACT 
            var sut = new Delivery(routeSpec.Object, itinerary, null);

            // ASSERT
            Assert.Equal(RoutingStatus.MisRouted, sut.RoutingStatus);
        }

        #endregion

        #region Last Known Status 

        [Theory]
        [AutoCargoData]
        public void Ctor_NoEvent_NoLastKnownLocation(
            RouteSpecification routeSpec
            , Itinerary itinerary)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(null, sut.LastKnownLocation);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_WithEvent_LastKnownLocationIsEventsLocation(
            RouteSpecification routeSpec
          , Itinerary itinerary
          , HandlingEvent @event)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(@event.Location, sut.LastKnownLocation);
        }

        #endregion

        #region Current Voyage

        [Theory]
        [AutoCargoData]
        public void Ctor_NoEvent_NoCurrentVoyage(
            RouteSpecification routeSpec
            , Itinerary itinerary
            , HandlingEvent @event)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(null, sut.CurrentVoyage);
        }

        [Theory]
        [InlineAutoCargoData(HandlingType.Claim)]
        [InlineAutoCargoData(HandlingType.Customs)]
        public void Ctor_EventWithNoVoyageNumber_NoCurrentVoyage(
            HandlingType type
            , RouteSpecification routeSpec
            , Itinerary itinerary
            , HandlingEvent @event)
        {
            // ARRANGE
            @event = @event
                .RecreateWith(type)
                .RecreateWith((VoyageNumber)null);

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(null, sut.CurrentVoyage);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_EventWithVoyageNumber_CurrentVoyageIsEventVoyage(
            RouteSpecification routeSpec
            , Itinerary itinerary
            , HandlingEvent @event)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(@event.Voyage, sut.CurrentVoyage);
        }

        #endregion

        #region Next Expected Handling Activity 

        [Theory]
        [AutoCargoData]
        public void Ctor_NoItinerary_NoActivity(
            RouteSpecification routeSpec
            , HandlingEvent @event)
        {
            // ACT
            var sut = new Delivery(routeSpec, null, @event);

            // ASSERT
            Assert.Equal(null, sut.NextExpectedHandlingActivity);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_MisRouted_NoActivity(
            Mock<IRouteSpecification> routeSpec,
            Itinerary itinerary,
            HandlingEvent @event,
            UnLocode location)
        {
            // ARRANGE
            routeSpec.SetupGet(m => m.Destination).Returns(location);           // *
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(false);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary, @event);

            // ASSERT
            Assert.Equal(null, sut.NextExpectedHandlingActivity);
        }


        [Theory]
        [AutoCargoData]
        public void Ctor_LastHandlingEventIsReceive_NextIsLoadAtItineraryFirstLoadLocationForFirstVoyage(
            Mock<IRouteSpecification> routeSpec,
            Itinerary itinerary,
            HandlingEvent @event,
            UnLocode location)
        {
            // ARRANGE
            @event = @event.RecreateWith(HandlingType.Receive);

            routeSpec.SetupGet(m => m.Destination).Returns(location);           // *
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(true);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Load, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(itinerary.FirstLoadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(itinerary.FirstYoyage, sut.NextExpectedHandlingActivity.Voyage);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_LastHandlingEventIsLoad_NextIsInUnloadAtSameLegUnloadLocationForLegsVoyage(
            Mock<IRouteSpecification> routeSpec,
            Mock<IItinerary> itinerary,
            HandlingEvent @event,
            UnLocode location,
            Leg leg)
        {
            // ARRANGE
            @event = @event.RecreateWith(HandlingType.Load);

            routeSpec.SetupGet(m => m.Destination).Returns(location);           // *
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary.Object)).Returns(true);

            itinerary.Setup(m => m.Of(It.IsAny<UnLocode>())).Returns(leg);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary.Object, @event);

            // ASSERT
            Assert.Equal(HandlingType.Unload, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(leg.UnloadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(leg.Voyage, sut.NextExpectedHandlingActivity.Voyage);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_LastHandlingEventIsUnloadOnItineraryLastUnloadLocation_NextIsCustomsOnSameLocationWithNoVoyage(
            Mock<IRouteSpecification> routeSpec,
            Itinerary itinerary,
            HandlingEvent @event,
            UnLocode location,
            Leg leg)
        {
            // ARRANGE
            @event = @event.RecreateWith(HandlingType.Unload)
                           .RecreateWith(itinerary.LastUnloadLocation);

            routeSpec.SetupGet(m => m.Destination).Returns(location);           // *
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(true);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Customs, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(itinerary.LastUnloadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(null, sut.NextExpectedHandlingActivity.Voyage);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_LastHandlingEventIsUnloadOnIntermediateLeg_NextIsLoadToNextLegLoadLocationAndVoyage(
            Mock<IRouteSpecification> routeSpec,
            Itinerary itinerary,
            HandlingEvent @event,
            UnLocode location)
        {
            // ARRANGE
            var legs = new List<Leg>(itinerary.Legs);

            @event = @event.RecreateWith(HandlingType.Unload)
                           .RecreateWith(legs[0].UnloadLocation);

            routeSpec.SetupGet(m => m.Destination).Returns(location);           // *
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(true);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Load, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(legs[1].LoadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(legs[1].Voyage, sut.NextExpectedHandlingActivity.Voyage);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_LastHandlingEventIsCustoms_NextIsClaimInItineraryLastUnloadLocationAndNoVoyage(
            Mock<IRouteSpecification> routeSpec,
            Itinerary itinerary,
            HandlingEvent @event,
            UnLocode location
            )
        {
            // ARRANGE
            @event = @event.RecreateWith(HandlingType.Customs);

            routeSpec.SetupGet(m => m.Destination).Returns(location);           // *
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(true);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Claim, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(itinerary.LastUnloadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(null, sut.NextExpectedHandlingActivity.Voyage);
        }


        [Theory]
        [AutoCargoData]
        public void Ctor_LastHandlingEventIsClaim_NoActivity(
            Mock<IRouteSpecification> routeSpec,
            Itinerary itinerary,
            HandlingEvent @event,
            UnLocode location)
        {
            // ARRANGE
            @event = @event.RecreateWith(HandlingType.Claim);

            routeSpec.SetupGet(m => m.Destination).Returns(location);
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary)).Returns(true);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary, @event);

            // ASSERT
            Assert.Equal(null, sut.NextExpectedHandlingActivity);
        }

        #endregion

        #region Is Unloaded At Destination

        [Theory]
        [AutoCargoData]
        public void Ctor_NoEvent_UnloadedAtDestinationIsFalse(
            RouteSpecification routeSpec,
            Itinerary itinerary)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(false, sut.IsUnloadedAtDestination);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_UnloadEventAtRouteSpecDestination_True(
            RouteSpecification routeSpec,
            Itinerary itinerary,
            HandlingEvent @event
            )
        {
            // ARRANGE
            @event = @event.RecreateWith(HandlingType.Unload)
                           .RecreateWith(routeSpec.Destination);

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(true, sut.IsUnloadedAtDestination);
        }

        #endregion

        #region Is Mishandled 

        [Theory]
        [AutoData]
        public void Ctor_NoEvent_IsMishandledIsFalse(
            RouteSpecification routeSpec,
            HandlingEvent @event)
        {
            // ACT
            var sut = new Delivery(routeSpec, null, @event);

            // ASSERT
            Assert.Equal(false, sut.IsMishandled);
        }


        [Theory]
        [AutoCargoData]
        public void Ctor_NoEvent_IsMishanledIsFalse(
            RouteSpecification routeSpec,
            Itinerary itinerary)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(false, sut.IsMishandled);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor_EventUnexpectedForItinerary_IsMishanledIsTrue(
            Mock<IRouteSpecification> routeSpec,
            Mock<IItinerary> itinerary,
            HandlingEvent @event,
            UnLocode location,
            Leg leg)
        {
            // ARRANGE
            @event = @event.RecreateWith(location);

            routeSpec.SetupGet(m => m.Destination).Returns(location);           // *
            routeSpec.Setup(m => m.IsSatisfiedBy(itinerary.Object)).Returns(true);

            itinerary.SetupGet(m => m.LastUnloadLocation).Returns(location);    // *
            itinerary.Setup(m => m.Of(@event.Location)).Returns(leg);           // *
            itinerary.Setup(m => m.IsExpected(@event)).Returns(false);

            // ACT
            var sut = new Delivery(routeSpec.Object, itinerary.Object, @event);

            // ASSERT
            Assert.Equal(false, sut.IsMishandled);
        }

        #endregion
    }
}

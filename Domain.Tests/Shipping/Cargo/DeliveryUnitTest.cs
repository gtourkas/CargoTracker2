using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using Moq;
using Xunit;

namespace Domain.Tests.Shipping.Cargo
{
    public class DeliveryUnitTest   
    {
        [Fact]
        public void Ctor__NoRoutSpecGiven__ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Delivery(null, null, null));
        }

        #region Transport Status 

        [Theory]
        [AutoCargoData]
        public void Ctor__NoEventGiven_TransportStatusSetToNoReceived(
            RouteSpecification routeSpec
            , Itinerary itinerary
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
        public void Ctor_HandlingTypeMappedToTransportStatusCorrectly(
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
        public void Ctor__NoItineraryGiven__RoutingStatusSetToNotRouted(
            RouteSpecification routeSpec
            )
        {
            // ACT
            var sut = new Delivery(routeSpec, null, null);

            // ASSERT
            Assert.Equal(RoutingStatus.NotRouted, sut.RoutingStatus);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor__RouteSpecGivenSatifiedByItineraryGiven__RoutingStatusSetToRouted(
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
        public void Ctor__RouteSpecGivenNotSatifiedByItineraryGiven__RoutingStatusSetToMisRouted(
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
        public void Ctor__NoEventGiven__LastKnownLocationSetToNone(
            RouteSpecification routeSpec
            , Itinerary itinerary)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Null(sut.LastKnownLocation);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor__EventGiven__LastKnownLocationIsSetToEventLocation(
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
        public void Ctor__NoEventGiven__CurrentVoyageSetToNone(
            RouteSpecification routeSpec
            , Itinerary itinerary)
        {
            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Null(sut.CurrentVoyage);
        }

        [Theory]
        [InlineAutoCargoData(HandlingType.Claim)]
        [InlineAutoCargoData(HandlingType.Customs)]
        public void Ctor__EventWithNoVoyageNumber_CurrentVoyageSetToNone(
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
            Assert.Null(sut.CurrentVoyage);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor__EventWithNoVoyageNumber_CurrentVoyageIsSetToEventVoyage(
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
        public void Ctor__NoItineraryGiven__NextExpectedActivitySetToNone(
            RouteSpecification routeSpec
            , HandlingEvent @event)
        {
            // ACT
            var sut = new Delivery(routeSpec, null, @event);

            // ASSERT
            Assert.Null(sut.NextExpectedHandlingActivity);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor__RouteSpecNotSatisifiedByItinerary__NextExpectedActivitySetToNone(
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
            Assert.Null(sut.NextExpectedHandlingActivity);
        }


        [Theory]
        [AutoCargoData]
        public void Ctor__LastHandlingEventGivenIsReceived__NextExpectedActivitySetToLoad_AtItineraryFirstLoadLocation_ForFirstVoyage(
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
        public void Ctor__LastHandlingEventGivenHasTypeLoad__NextExpectedActivitySetToUnload_AtSameLegUnloadLocation_ForLegsVoyage(
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
        public void Ctor__LastHandlingEventGivenIsUnloadOnItineraryLastUnloadLocation__NextExpectedActivitySetToCustomsOnSameLocationWithNoVoyage(
            Mock<IRouteSpecification> routeSpec,
            Itinerary itinerary,
            HandlingEvent @event,
            UnLocode location)
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
            Assert.Null(sut.NextExpectedHandlingActivity.Voyage);
        }

        [Theory]
        [AutoCargoData]
        public void Ctor__LastHandlingEventGvivenIsUnloadOnIntermediateLeg__NextIsLoadToNextLegLoadLocationAndVoyage(
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
            Assert.Null(sut.NextExpectedHandlingActivity.Voyage);
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
            Assert.Null(sut.NextExpectedHandlingActivity);
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
            Assert.False(sut.IsUnloadedAtDestination);
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
            Assert.True(sut.IsUnloadedAtDestination);
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
            Assert.False(sut.IsMishandled);
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
            Assert.False(sut.IsMishandled);
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
            Assert.False(sut.IsMishandled);
        }

        #endregion
    }
}

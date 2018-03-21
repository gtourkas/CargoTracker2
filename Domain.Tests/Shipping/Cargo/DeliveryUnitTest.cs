using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using Domain.Tests.Shipping.Cargo.Infra;
using Moq;
using Xunit;

namespace Domain.Tests.Shipping.Cargo
{
    public class DeliveryUnitTest   
    {
        [Fact]
        public void Ctor__NoRouteSpecGiven__ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Delivery(null, null, null));
        }

        #region Transport Status 

        [Fact]
        public void Ctor__NoEventGiven__TransportStatusSetToNoReceived()
        {
            // ARRANGE
            var routeSpec = new Fixture().Create<RouteSpecification>();
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(TransportStatus.NotReceived, sut.TransportStatus);
        }


        [Theory]
        [InlineData(HandlingType.Load, TransportStatus.OnBoardVessel)]
        [InlineData(HandlingType.Unload, TransportStatus.InPort)]
        [InlineData(HandlingType.Receive, TransportStatus.InPort)]
        [InlineData(HandlingType.Customs, TransportStatus.InPort)]
        [InlineData(HandlingType.Claim, TransportStatus.Claimed)]
        public void Ctor__HandlingTypeMappedToTransportStatusCorrectly(
            HandlingType type
            , TransportStatus status)
        {
            // ARRANGE
            var routeSpec = new Fixture().Create<RouteSpecification>();
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(type));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(status, sut.TransportStatus);
        }

        #endregion

        #region Routing Status 

        [Theory]
        [AutoData]
        public void Ctor__NoItineraryGiven__RoutingStatusSetToNotRouted(
            RouteSpecification routeSpec
            )
        {
            // ACT
            var sut = new Delivery(routeSpec, null, null);

            // ASSERT
            Assert.Equal(RoutingStatus.NotRouted, sut.RoutingStatus);
        }

        [Fact]
        public void Ctor__RouteSpecGivenSatifiedByItineraryGiven__RoutingStatusSetToRouted()
        {
            // ARRANGE
            var fixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = fixture.Create<RouteSpecification>();
            var itinerary = fixture.Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(RoutingStatus.Routed, sut.RoutingStatus);
        }

        [Theory]
        [AutoData]
        public void Ctor__RouteSpecGivenNotSatifiedByItineraryGiven__RoutingStatusSetToMisRouted(
            RouteSpecification routeSpec)
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT 
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Equal(RoutingStatus.MisRouted, sut.RoutingStatus);
        }

        #endregion

        #region Last Known Status 

        [Theory]
        [AutoData]
        public void Ctor__NoEventGiven__LastKnownLocationSetToNone(
            RouteSpecification routeSpec
            )
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Null(sut.LastKnownLocation);
        }

        [Theory]
        [AutoData]
        public void Ctor__EventGiven__LastKnownLocationIsSetToTheEventLocation(
            RouteSpecification routeSpec
          , HandlingEvent @event)
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(@event.Location, sut.LastKnownLocation);
        }

        #endregion

        #region Current Voyage

        [Theory]
        [AutoData]
        public void Ctor__NoEventGiven__CurrentVoyageSetToNone(
            RouteSpecification routeSpec)
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.Null(sut.CurrentVoyage);
        }

        [Theory]
        [InlineAutoData(HandlingType.Claim)]
        [InlineAutoData(HandlingType.Customs)]
        public void Ctor__EventWithNoVoyageNumber__CurrentVoyageSetToNone(
            HandlingType type
            , RouteSpecification routeSpec)
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(type).With((VoyageNumber)null));

            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Null(sut.CurrentVoyage);
        }

        [Theory]
        [AutoData]
        public void Ctor__EventWithAVoyageNumber__CurrentVoyageIsSetToEventVoyage(
            RouteSpecification routeSpec
            , HandlingEvent @event)
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(@event.Voyage, sut.CurrentVoyage);
        }

        #endregion

        #region Next Expected Handling Activity 

        [Theory]
        [AutoData]
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
        [AutoData]
        public void Ctor__RouteSpecNotSatisifiedByItinerary__NextExpectedActivitySetToNone(
            RouteSpecification routeSpec,
            HandlingEvent @event)
        {
            // ARRANGE
            var itinerary = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Null(sut.NextExpectedHandlingActivity);
        }


        [Fact]
        public void Ctor__LastHandlingEventGivenHasTypeReceived__NextExpectedActivitySetToLoad_AtItineraryFirstLoadLocation_OfFirstVoyage()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Receive));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Load, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(itinerary.FirstLoadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(itinerary.FirstYoyage, sut.NextExpectedHandlingActivity.Voyage);
        }

        [Fact]
        public void Ctor__LastHandlingEventGivenHasTypeLoad__NextExpectedActivitySetToUnload_AtSameLegsUnloadLocation_OfLegsVoyage()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var randomLeg = new List<Leg>(itinerary.Legs)[new Random().Next(0, itinerary.Legs.Count)];

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Load).With(randomLeg.LoadLocation));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Unload, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(randomLeg.UnloadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(randomLeg.Voyage, sut.NextExpectedHandlingActivity.Voyage);
        }

        [Fact]
        public void Ctor__LastHandlingEventGivenIsUnloadOnItineraryLastUnloadLocation__NextExpectedActivitySetToCustoms_AtSameLocation_WithNoVoyage()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Unload).With(itinerary.LastUnloadLocation));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Customs, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(itinerary.LastUnloadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Null(sut.NextExpectedHandlingActivity.Voyage);
        }

        [Fact]
        public void Ctor__LastHandlingEventGivenIsUnloadOnIntermediateLeg__NextExpectedActivitySetToLoad_AtNextVoyagesFirstLegLoadLocation()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var legs = new List<Leg>(itinerary.Legs);

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Unload).With(legs[0].UnloadLocation));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Load, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(legs[1].LoadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Equal(legs[1].Voyage, sut.NextExpectedHandlingActivity.Voyage);
        }

        [Fact]
        public void Ctor__LastHandlingEventGivenIsCustoms__NextExpectedActivitySetToClaim_AtItineraryLastUnloadLocation_WithNoVoyage()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Customs));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Equal(HandlingType.Claim, sut.NextExpectedHandlingActivity.Type);
            Assert.Equal(itinerary.LastUnloadLocation, sut.NextExpectedHandlingActivity.Location);
            Assert.Null(sut.NextExpectedHandlingActivity.Voyage);
        }


        [Fact]
        public void Ctor__LastHandlingEventGivenIsClaim__NoNextExpectedActivity()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Claim));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.Null(sut.NextExpectedHandlingActivity);
        }

        #endregion

        #region Is Unloaded At Destination

        [Fact]
        public void Ctor__NoEventGiven__UnloadedAtDestinationSetToFalse()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.False(sut.IsUnloadedAtDestination);
        }

        [Fact]
        public void Ctor__UnloadEventGivenAtRouteSpecDestination__UnloadedAtDestinationSetToTrue()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Unload).With(routeSpec.Destination));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.True(sut.IsUnloadedAtDestination);
        }

        #endregion

        #region Is Mishandled 

        [Theory]
        [AutoData]
        public void Ctor__NoItinenaryGiven__IsMishandledSetToFalse(
            RouteSpecification routeSpec,
            HandlingEvent @event)
        {
            // ACT
            var sut = new Delivery(routeSpec, null, @event);

            // ASSERT
            Assert.False(sut.IsMishandled);
        }

        [Fact]
        public void Ctor__NoEventGiven__IsMishanledSetToFalse()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, null);

            // ASSERT
            Assert.False(sut.IsMishandled);
        }

        [Fact]
        public void Ctor__EventUnexpectedForItineraryGiven__IsMishanledSetToTrue()
        {
            // ARRANGE
            var routeSpecAndItineraryFixture = new Fixture().Customize(new RouteSpecAndSatisfyingItineraryCustomization());
            var routeSpec = routeSpecAndItineraryFixture.Create<RouteSpecification>();
            var itinerary = routeSpecAndItineraryFixture.Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(routeSpec.Destination));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var sut = new Delivery(routeSpec, itinerary, @event);

            // ASSERT
            Assert.False(sut.IsMishandled);
        }

        #endregion
    }
}

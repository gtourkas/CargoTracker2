using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Domain.Shipping.Cargo;
using Domain.Tests.Shipping.Cargo.Infra;
using Xunit;

namespace Domain.Tests.Shipping.Cargo
{
    public class ItineraryUnitTest
    {
        [Fact]
        public void Ctor__NoLegsGiven__ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Itinerary(null));
        }

        [Fact]
        public void Ctor__EmptyLegsGiven__ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Itinerary(new List<Leg>()));
        }

        [Fact]
        public void Ctor()
        {
            // ARRANGE
            var legs = new List<Leg>(new Fixture().Customize(new DefaultLegCustomization()).CreateMany<Leg>());

            // ACT
            var itinerary = new Itinerary(legs);

            // ASSERT
            var firstLeg = legs.First();
            Assert.Equal(firstLeg.LoadLocation, itinerary.FirstLoadLocation);
            Assert.Equal(firstLeg.Voyage, itinerary.FirstYoyage);

            var lastLeg = legs.Last();
            Assert.Equal(lastLeg.UnloadLocation, itinerary.LastUnloadLocation);
            Assert.Equal(lastLeg.UnloadTime, itinerary.FinalArrivalDate);
        }

        [Fact]
        public void IsExpected__NoEventGiven__ReturnsTrue()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var r = sut.IsExpected(null);

            // ASSERT
            Assert.True(r);
        }

        [Fact]
        public void IsExpected__ReceiveEventGiven_AtFirstLoadingLocation__ReturnsTrue()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Load).With(sut.FirstLoadLocation));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var r = sut.IsExpected(@event);

            // ASSERT
            Assert.True(r);
        }

        [Fact]
        public void IsExpected__LoadEventGiven_AtAnyLoadingLocation__ReturnsTrue()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            var anyLoadingLocation = new List<Leg>(sut.Legs)[new Random().Next(0, sut.Legs.Count - 1)].LoadLocation;

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Load).With(anyLoadingLocation));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var r = sut.IsExpected(@event);

            // ASSERT
            Assert.True(r);
        }

        [Fact]
        public void IsExpected__UnLoadEventGiven_AtAnyUnLoadingLocation__ReturnsTrue()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            var anyUnLoadingLocation = new List<Leg>(sut.Legs)[new Random().Next(0, sut.Legs.Count - 1)].UnloadLocation;

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(HandlingType.Unload).With(anyUnLoadingLocation));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var r = sut.IsExpected(@event);

            // ASSERT
            Assert.True(r);
        }

        [Theory]
        [InlineData(HandlingType.Claim)]
        [InlineData(HandlingType.Customs)]
        public void IsExpected__EventClaimsOrCustomGiven_AtFinalUnloadingLocation__ReturnsTrue(
            HandlingType type)
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            var eventFixture = new Fixture();
            eventFixture.Customizations.Add(new HandlingEventBuilder().With(type).With(sut.LastUnloadLocation));
            var @event = eventFixture.Create<HandlingEvent>();

            // ACT
            var r = sut.IsExpected(@event);

            // ASSERT
            Assert.True(r);
        }

        [Fact]
        public void NextOf__TheFirst__IsTheSecond()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();
            var legs = new List<Leg>(sut.Legs);

            // ACT
            var l = sut.NextOf(legs[0].UnloadLocation);

            // ASSERT
            Assert.Equal(legs[1], l);
        }

        [Fact]
        public void NextOf__Last__IsNoLeg()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();

            // ACT
            var l = sut.NextOf(sut.LastUnloadLocation);

            // ASSERT
            Assert.Null(l);
        }

        [Fact]
        public void Of()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultItineraryCustomization()).Create<Itinerary>();
            var legs = new List<Leg>(sut.Legs);

            foreach (var l in legs)
            {
                // ACT
                var r1 = sut.Of(l.LoadLocation);
                var r2 = sut.Of(l.UnloadLocation);

                // ASSERT
                Assert.Equal(l, r1);
                Assert.Equal(l, r2);
            }
        }
    }
}
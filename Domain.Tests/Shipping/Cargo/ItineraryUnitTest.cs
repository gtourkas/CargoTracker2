using System;
using System.Collections.Generic;
using System.Text;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Domain.Shipping;
using Ploeh.AutoFixture;

namespace Domain.Tests.Shipping.Cargo
{
    public class ItineraryUnitTest
    {
        [Fact]
        public void Ctor_NoLegs_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Itinerary(null));
        }

        [Fact]
        public void Ctor_EmptyLegs_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Itinerary(new List<Leg>()));
        }

        [Theory]
        [AutoCargoData]
        public void Ctor(Itinerary sut, List<Leg> legs)
        {
            // ACT
            var itinerary = new Itinerary(legs);

            // ASSERT
            Assert.Equal(legs[0].LoadLocation, itinerary.FirstLoadLocation);
            Assert.Equal(legs[0].Voyage, itinerary.FirstYoyage);

            Assert.Equal(legs[legs.Count - 1].UnloadLocation, itinerary.LastUnloadLocation);
            Assert.Equal(legs[legs.Count - 1].UnloadTime, itinerary.FinalArrivalDate);
        }

        [Theory]
        [AutoCargoData]
        public void IsExpected_NoEvent_ReturnsTrue(Itinerary sut)
        {
            // ACT
            var r = sut.IsExpected(null);

            // ASSERT
            Assert.Equal(true, r);
        }

        [Theory]
        [AutoCargoData]
        public void IsExpected_ReceiveEventAtFirstLoadingLocation_ReturnsTrue(Itinerary sut
            , HandlingEvent @event)
        {
            // ARRANGE
            @event = @event
                .RecreateWith(HandlingType.Load)
                .RecreateWith(sut.FirstLoadLocation);

            // ACT
            var r = sut.IsExpected(@event);

            // ASSERT
            Assert.Equal(true, r);
        }

        [Theory]
        [AutoCargoData]
        public void IsExpected_LoadEventAtALoadingLocation_ReturnsTrue(Itinerary sut
            , HandlingEvent @event)
        {
            // ARRANGE
            var legs = new List<Leg>(sut.Legs);

            foreach (var l in legs)
            {
                @event = @event
                    .RecreateWith(HandlingType.Load)
                    .RecreateWith(l.LoadLocation);

                // ACT
                var r = sut.IsExpected(@event);

                // ASSERT
                Assert.Equal(true, r);
            }
        }

        [Theory]
        [AutoCargoData]
        public void IsExpected_LoadEventAtAUnloadingLocation_ReturnsTrue(Itinerary sut
                , HandlingEvent @event)
        {
            // ARRANGE
            var legs = new List<Leg>(sut.Legs);

            foreach (var l in legs)
            {
                @event = @event
                    .RecreateWith(HandlingType.Load)
                    .RecreateWith(l.LoadLocation);

                // ACT
                var r = sut.IsExpected(@event);

                // ASSERT
                Assert.Equal(true, r);
            }
        }

        [Theory]
        [InlineAutoCargoData(HandlingType.Claim)]
        [InlineAutoCargoData(HandlingType.Customs)]
        public void IsExpected_ClaimsOrCustomEventAtAFinalUnloadingLocation_ReturnsTrue(
            HandlingType type
            , Itinerary sut
            , HandlingEvent @event)
        {
            // ARRANGE
            @event = @event
                .RecreateWith(type)
                .RecreateWith(sut.LastUnloadLocation);

            // ACT
            var r = sut.IsExpected(@event);

            // ASSERT
            Assert.Equal(true, r);
        }

        [Theory]
        [AutoCargoData]
        public void NextOf_First_IsSecond(Itinerary sut)
        {
            var legs = new List<Leg>(sut.Legs);

            // ACT
            var l = sut.NextOf(legs[0].UnloadLocation);

            // ASSERT
            Assert.Equal(legs[1], l);
        }

        [Theory]
        [AutoCargoData]
        public void NextOf_Last_IsNoLeg(Itinerary sut)
        {
            // ARRANGE
            var legs = new List<Leg>(sut.Legs);

            // ACT
            var l = sut.NextOf(sut.LastUnloadLocation);

            // ASSERT
            Assert.Equal(null, l);
        }

        [Theory]
        [AutoCargoData]
        public void Of(Itinerary sut)
        {
            // ARRANGE
            var legs = new List<Leg>(sut.Legs);

            for (var i = 0; i < legs.Count; i++)
            {
                var l = legs[i];

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
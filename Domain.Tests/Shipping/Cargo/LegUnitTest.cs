using System;
using AutoFixture.Xunit2;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using Xunit;

namespace Domain.Tests.Shipping.Cargo
{
    public class LegUnitTest
    {
        [Theory]
        [AutoData]
        public void Ctor__NoVoyageGiven__ThrowsArgumentNullException(
            UnLocode loadLocation,
            UnLocode unloadLocation,
            DateTime loadTime,
            DateTime unloadTime
            )
        {
            Assert.Throws<ArgumentNullException>( ()=>new Leg(null, loadLocation, unloadLocation, loadTime, unloadTime));
        }

        [Theory]
        [AutoData]
        public void Ctor__NoLoadLocationGiven__ThrowsArgumentNullException(
            VoyageNumber voyage,
            UnLocode unloadLocation,
            DateTime loadTime,
            DateTime unloadTime
            )
        {
            Assert.Throws<ArgumentNullException>(() => new Leg(voyage, null, unloadLocation, loadTime, unloadTime));
        }

        [Theory]
        [AutoData]
        public void Ctor__NoUnLoadLocationGiven__ThrowsArgumentNullException(
            VoyageNumber voyage,
            UnLocode loadLocation,
            DateTime loadTime,
            DateTime unloadTime
            )
        {
            Assert.Throws<ArgumentNullException>(() => new Leg(voyage, loadLocation, null, loadTime, unloadTime));
        }

        [Theory]
        [AutoData]
        public void Ctor__UnLoadTimeGiven_IsEarlierThanLoadTime__ThrowsArgumentException(
            VoyageNumber voyage,
            UnLocode loadLocation,
            UnLocode unloadLocation,
            DateTime loadTime
            )
        {
            Assert.Throws<ArgumentException>(() => new Leg(voyage, loadLocation, unloadLocation, loadTime, loadTime.Subtract(TimeSpan.FromSeconds(1))));
        }
    }
}

using Domain.Shipping;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests.Shipping.Cargo
{
    public class RouteSpecificationUnitTest
    {
        [Theory]
        [AutoData]
        public void Ctor_NoOrigin_ThrowsArgumentNullException(
            UnLocode location
            )
        {
            Assert.Throws<ArgumentNullException>(() => new RouteSpecification(null, location, DateTime.UtcNow));
        }

        [Theory]
        [AutoData]
        public void Ctor_NoDestination_ThrowsArgumentNullException(
            UnLocode location
            )
        {
            Assert.Throws<ArgumentNullException>(() => new RouteSpecification(location, null, DateTime.UtcNow));
        }

        [Theory]
        [AutoData]
        public void Ctor_SameOrigianAndDestination_ThrowsInvalidOperationException(
            UnLocode location
            )
        {
            Assert.Throws<InvalidOperationException>(() => new RouteSpecification(location, location, DateTime.UtcNow));
        }

        [Theory]
        [AutoCargoData]
        public void IsSatisfiedBy_OriginDifferentThanItineraryFirstLoadingLocation_ReturnFalse(
            RouteSpecification sut,
            Itinerary itinerary
            )
        {
            // ACT
            var r = sut.IsSatisfiedBy(itinerary);

            // ASSERT
            Assert.Equal(false, r);
        }

        [Theory]
        [AutoData]
        public void IsSatisfiedBy_DestinationDifferentThanItineraryLastUnLoadingLocation_ReturnFalse(
            RouteSpecification sut,
            Mock<IItinerary> itinerary,
            UnLocode location
        )
        {
            // ARRANGE 
            itinerary.SetupGet(m => m.FirstLoadLocation).Returns(sut.Origin);
            itinerary.SetupGet(m => m.LastUnloadLocation).Returns(location);

            // ACT
            var r = sut.IsSatisfiedBy(itinerary.Object);

            // ASSERT
            Assert.Equal(false, r);
        }

        [Theory]
        [AutoData]
        public void IsSatisfiedBy_FinalArrivalDateLaterThanArrivalDeadline_ReturnFalse(
                RouteSpecification sut,
                Mock<IItinerary> itinerary
            )
        {
            // ARRANGE 
            itinerary.SetupGet(m => m.FirstLoadLocation).Returns(sut.Origin);
            itinerary.SetupGet(m => m.LastUnloadLocation).Returns(sut.Destination);
            itinerary.SetupGet(m => m.FinalArrivalDate).Returns(sut.ArrivalDeadline.AddSeconds(1));

            // ACT
            var r = sut.IsSatisfiedBy(itinerary.Object);

            // ASSERT
            Assert.Equal(false, r);
        }


        [Theory]
        [AutoData]
        public void IsSatisfiedBy_ReturnsTrue(
        RouteSpecification sut,
        Mock<IItinerary> itinerary
    )
        {
            // ARRANGE 
            itinerary.SetupGet(m => m.FirstLoadLocation).Returns(sut.Origin);
            itinerary.SetupGet(m => m.LastUnloadLocation).Returns(sut.Destination);
            itinerary.SetupGet(m => m.FinalArrivalDate).Returns(sut.ArrivalDeadline.Subtract(TimeSpan.FromSeconds(1)));

            // ACT
            var r = sut.IsSatisfiedBy(itinerary.Object);

            // ASSERT
            Assert.Equal(true, r);
        }
    }
}

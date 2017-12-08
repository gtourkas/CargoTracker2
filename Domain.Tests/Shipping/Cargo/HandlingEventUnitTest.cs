using Domain.Shipping;
using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests.Shipping.Cargo
{
    public class HandlingEventUnitTest
    {
        [Theory]
        [AutoData]
        public void Ctor_NoTrackingId_ThrowsArgumentNullException(
            HandlingType type,
            UnLocode location,
            VoyageNumber voyage,
            DateTime completed,
            DateTime registered
            )
        {
            Assert.Throws<ArgumentNullException>(() => new HandlingEvent(
                null,
                type,
                location,
                voyage,
                completed,
                registered
                ));
        }

        [Theory]
        [AutoData]
        public void Ctor_NoLocation_ThrowsArgumentNullException(
            TrackingId trackingId,
            HandlingType type,
            VoyageNumber voyage,
            DateTime completed,
            DateTime registered
            )
        {
            Assert.Throws<ArgumentNullException>(() => new HandlingEvent(
                trackingId,
                type,
                null,
                voyage,
                completed,
                registered
                ));
        }

        [Theory]
        [InlineAutoData(HandlingType.Load)]
        [InlineAutoData(HandlingType.Unload)]
        public void Ctor_UnloadOrLoadAndNoVoyage_ThrowsArgumentNullException(
            HandlingType type,
            TrackingId trackingId,
            UnLocode location,
            DateTime completed,
            DateTime registered
            )
        {
            Assert.Throws<InvalidOperationException>(() => new HandlingEvent(
                trackingId,
                type,
                location,
                null,
                completed,
                registered
                ));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Domain.Monitoring.TempRangeMonitor;
using Domain.Tests.Monitoring.TempRangeMonitor.Infra;
using Xunit;
using AutoFixture;
using AutoFixture.Xunit2;

namespace Domain.Tests.Monitoring.TempRangeMonitor
{
    public class RangeUnitTest
    {
        [Fact]
        public void IsOut__TempOutRange__ReturnsTrue()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultRangeCustomization()).Create<Range>();
            var temp = new Fixture().Customize(new RangeTempCustomization(Temp.MinValue, (int)sut.From.Value - 1)).Create<Temp>();

            // ACT
            var r = sut.IsOut(temp);

            // ASSERT
            Assert.True(r);
        }

        [Fact]
        public void IsOut__TempInRange__ReturnsFalse()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultRangeCustomization()).Create<Range>();
            var temp = new Fixture().Customize(new RangeTempCustomization((int)sut.From.Value, (int)sut.Till.Value)).Create<Temp>();

            // ACT
            var r = sut.IsOut(temp);

            // ASSERT
            Assert.False(r);
        }
    }
}

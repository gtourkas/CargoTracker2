using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture.Xunit2;
using Domain.Monitoring.TempRangeMonitor;
using FluentAssertions;
using Xunit;

namespace Domain.Tests.Monitoring.TempRangeMonitor
{
    public class ReadingsCollectionUnitTest
    {
        [Theory]
        [AutoReadingData]
        public void Add__NoOtherItems__ItemAdded_and_DurationIsZero(ReadingsCollection sut, Reading reading)
        {
            // ACT
            sut.Add(reading);

            // ASSERT
            sut.Items.First().Should().BeEquivalentTo(reading);
            sut.TotalDuration.Should().BeEquivalentTo(Duration.Zero);
        }

        [Theory]
        [AutoReadingData]
        public void Add__OtherItemsExist__ItemAdded_and_DurationSumsToItemsTimeDiff(ReadingsCollection sut, Reading current, Reading next)
        {
            // ARRANGE
            sut.Add(current);

            // ACT
            sut.Add(next);

            // ASSERT
            sut.Items.Last().Should().BeEquivalentTo(next);
            sut.TotalDuration.Should().BeEquivalentTo(new Duration(next.Timestamp.Subtract(current.Timestamp)));
        }

        [Theory]
        [AutoReadingData]
        public void Clear__ItemsExist__ItemsCleared_and_DurationIsZero(ReadingsCollection sut, Reading current, Reading next)
        {
            // ARRANGE
            sut.Add(current);
            sut.Add(next);

            // ACT
            sut.Clear();

            // ASSERT
            sut.Items.Should().BeEmpty();
            sut.TotalDuration.Should().BeEquivalentTo(Duration.Zero);
        }
    }
}

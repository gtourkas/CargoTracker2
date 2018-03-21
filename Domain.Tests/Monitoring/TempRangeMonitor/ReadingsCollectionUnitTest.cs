using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Monitoring.TempRangeMonitor;
using Domain.Tests.Monitoring.TempRangeMonitor.Infra;
using FluentAssertions;
using Xunit;

namespace Domain.Tests.Monitoring.TempRangeMonitor
{
    public class ReadingsCollectionUnitTest
    {
        [Theory]
        [AutoData]
        public void Add__NoOtherItemsExist__ItemAdded_and_DurationIsZero(ReadingsCollection sut)
        {
            // ARRANGE
            var reading = new Fixture().Customize(new DefaultTempCustomization()).Create<Reading>();

            // ACT
            sut.Add(reading);

            // ASSERT
            sut.Items.First().Should().BeEquivalentTo(reading);
            sut.TotalDuration.Should().BeEquivalentTo(Duration.Zero);
        }

        [Theory]
        [AutoData]
        public void Add__OtherItemsExist__ItemAdded_and_DurationSumsToItemsTimeDiff(ReadingsCollection sut)
        {
            // ARRANGE
            var readingFixture = new Fixture().Customize(new DefaultTempCustomization());
            var current = readingFixture.Create<Reading>();
            var next = readingFixture.Create<Reading>();

            sut.Add(current);

            // ACT
            sut.Add(next);

            // ASSERT
            sut.Items.Last().Should().BeEquivalentTo(next);
            sut.TotalDuration.Should().BeEquivalentTo(new Duration(next.Timestamp.Subtract(current.Timestamp)));
        }

        [Theory]
        [AutoData]
        public void Clear__ItemsExist__ItemsCleared_and_DurationIsZero(ReadingsCollection sut)
        {
            // ARRANGE
            var readingFixture = new Fixture().Customize(new DefaultTempCustomization());
            var current = readingFixture.Create<Reading>();
            var next = readingFixture.Create<Reading>();
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

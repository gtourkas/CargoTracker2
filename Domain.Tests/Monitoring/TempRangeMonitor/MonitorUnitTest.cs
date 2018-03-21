using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Domain.Monitoring.TempRangeMonitor;
using Domain.Monitoring.TempRangeMonitor.Events;
using Domain.Tests.Monitoring.TempRangeMonitor.Infra;
using FluentAssertions;
using Machine.Specifications;
using Xunit;
using System.Collections.Generic;
using AutoFixture.Xunit2;

namespace Domain.Tests.Monitoring.TempRangeMonitor
{
    public class MonitorUnitTest
    {
        [Fact]
        public void Check__LastReadingUpdated()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultMonitorCustomization()).Create<Monitor>();
            var reading = new Fixture().Customize(new DefaultReadingCustomization()).Create<Reading>();

            // ACT
            sut.Check(reading);

            // ASSERT
            reading.Should().BeEquivalentTo(sut.LastReading);
        }

        [Fact]
        public void Check__ReadingsGivenAreOutOfSpecRange_ButWithLessThanSpecDuration__AlarmIsNotStarted()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultMonitorCustomization()).Create<Monitor>();

            var readings = new Fixture()
                .Customize(new ReadingsCollectionsCustomization(
                    Temp.MinValue
                    , (int) sut.Specification.Range.From.Value - 1
                    , sut.Specification.Duration.Value.TotalSeconds
                    , false
                )).Create<ReadingsCollection>();

            // ACT
            foreach (var i in readings.Items)
                sut.Check(i);

            // ASSERT
            Assert.False(sut.AlarmStarted);
        }

        [Fact]
        public void Check__ReadingsGivenAreOutOfSpecRange_AndWithMoreThanSpecDuration__AlarmIsStarted_and_AlarmStartedEventEmitted()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultMonitorCustomization()).Create<Monitor>();

            var readings = new Fixture()
                .Customize(new ReadingsCollectionsCustomization(
                    Temp.MinValue
                    , (int)sut.Specification.Range.From.Value - 1
                    , sut.Specification.Duration.Value.TotalSeconds
                    , true
                )).Create<ReadingsCollection>();

            // ACT
            foreach (var i in readings.Items)
                sut.Check(i);

            // ASSERT
            Assert.True(sut.AlarmStarted);
            sut.Events[0].Should().BeEquivalentTo(new AlarmStarted(sut.ContainerId, readings.Items.Last()));
        }


        [Fact]
        public void Check__AlarmIsStarted_and_GivenReadingWithinSpec__AlarmIsStopped_and_AlarmStoppedEventEmitted_and_ConsecOutOfRangeReadingsEmptied()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultMonitorCustomization()).Create<Monitor>();

            var readings = new Fixture()
                .Customize(new ReadingsCollectionsCustomization(
                    Temp.MinValue
                    , (int)sut.Specification.Range.From.Value - 1
                    , sut.Specification.Duration.Value.TotalSeconds
                    , true
                 )).Create<ReadingsCollection>();
            foreach (var i in readings.Items)
                sut.Check(i);

            var reading = new Fixture().Customize(new RangeReadingCustomization(
                    (int) sut.Specification.Range.From.Value
                    , (int) sut.Specification.Range.Till.Value))
                .Create<Reading>();

            // ACT
            sut.Check(reading);

            // ASSERT
            Assert.False(sut.AlarmStarted);
            sut.Events[1].Should().BeEquivalentTo(new AlarmStopped(sut.ContainerId, reading));
            Assert.Empty(sut.ConsecOutOfRangeReadings.Items);
        }
    }

}
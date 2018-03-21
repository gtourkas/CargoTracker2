using Domain.Monitoring.WeightShiftMonitor;
using Domain.Tests.Monitoring.WeightShiftMonitor.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;
using FluentAssertions;
using Domain.Monitoring.WeightShiftMonitor.Events;

namespace Domain.Tests.Monitoring.WeightShiftMonitor
{
    public class MonitorUnitTest
    {

        [Fact]
        public void Check__ReadingIsOutOfSpec_AndNoAlarmIsStarted__AlarmIsStarted_and_AlarmStartedEventEmitted()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultMonitorCustomization()).Create<Monitor>();

            var reading = new Fixture().Customize(new DefaultReadingCustomization(
                sut.InitialReading.Weights.Select(_ => _.Value).ToArray()
                , sut.Specification.Percentage.Value
                , true
                )).Create<Reading>();

            // ACT
            sut.Check(reading);

            // ASSERT
            var shift = sut.InitialReading.CalcShift(reading);
            Assert.True(sut.AlarmStarted);
            sut.Events[0].Should().BeEquivalentTo(new AlarmStarted(sut.ContainerId, reading, shift.PercOfLargestShift, shift.DirOfLargestShift));
        }


        [Fact]
        public void Check__ReadingIsOutOfSpec_ButAlarmIsAlreadyStarted__NoEventEmitted()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultMonitorCustomization()).Create<Monitor>();

            var reading = new Fixture().Customize(new DefaultReadingCustomization(
                sut.InitialReading.Weights.Select(_ => _.Value).ToArray()
                , sut.Specification.Percentage.Value
                , true
            )).Create<Reading>();
            sut.Check(reading);     // alarm is started

            sut.Events.Clear();

            var nextReading = new Fixture().Customize(new DefaultReadingCustomization(
                sut.InitialReading.Weights.Select(_ => _.Value).ToArray()
                , sut.Specification.Percentage.Value
                , true
            )).Create<Reading>();

            // ACT
            sut.Check(nextReading);

            // ASSERT
            sut.Events.Should().BeEmpty();
        }

        [Fact]
        public void Check__ReadingIsInSpec_AndAlarmIsAlreadyStarted__AlarmIsStopped_and_AlarmStoppedEventEmitted()
        {
            // ARRANGE
            var sut = new Fixture().Customize(new DefaultMonitorCustomization()).Create<Monitor>();

            var reading = new Fixture().Customize(new DefaultReadingCustomization(
                sut.InitialReading.Weights.Select(_ => _.Value).ToArray()
                , sut.Specification.Percentage.Value
                , true
            )).Create<Reading>();
            sut.Check(reading);     // alarm is started

            sut.Events.Clear();

            var nextReading = new Fixture().Customize(new DefaultReadingCustomization(
                sut.InitialReading.Weights.Select(_ => _.Value).ToArray()
                , sut.Specification.Percentage.Value
                , false
            )).Create<Reading>();

            // ACT
            sut.Check(nextReading);

            // ASSERT
            Assert.False(sut.AlarmStarted);
            sut.Events[0].Should().BeEquivalentTo(new AlarmStopped(sut.ContainerId, nextReading));
        }
    }
}

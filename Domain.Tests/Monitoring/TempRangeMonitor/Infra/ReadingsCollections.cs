using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.Kernel;
using Domain.Monitoring.TempRangeMonitor;
using System.Linq;
using System.Reflection;

namespace Domain.Tests.Monitoring.TempRangeMonitor.Infra
{
    public class ReadingsCollectionsBuilder : ISpecimenBuilder
    {
        private double _duration;

        private bool _durationShouldBeExceeded;

        public ReadingsCollectionsBuilder(double duration, bool durationShouldBeExceeded)
        {
            _duration = duration;
            _durationShouldBeExceeded = durationShouldBeExceeded;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var paramInfo = request as ParameterInfo;
            if (paramInfo != null
                && paramInfo.Name == "items")
            {
                var readings = new List<Reading>();
                while (readings.Count == 0  || readings.Last().Timestamp.Subtract(readings.First().Timestamp).TotalSeconds < _duration)
                {
                    var reading = context.Create<Reading>();

                    if (!_durationShouldBeExceeded && readings.Count > 0  && (reading.Timestamp.Subtract(readings.First().Timestamp).TotalSeconds >= _duration))
                        break;

                    readings.Add(reading);
                }

                return readings;
            }

            return new NoSpecimen();
        }
    }

    public class ReadingsCollectionsCustomization : ICustomization
    {
        private int _tempFrom;

        private int _tempTill;

        private double _duration;

        private bool _durationShouldBeExceeded;

        public ReadingsCollectionsCustomization(int tempFrom, int tempTill, double duration, bool durationShouldBeExceeded)
        {
            _tempFrom = tempFrom;
            _tempTill = tempTill;
            _duration = duration;
            _durationShouldBeExceeded = durationShouldBeExceeded;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customize(new RangeTempCustomization(_tempFrom, _tempTill));
            fixture.Customizations.Add(new StrictlyMonotonicallyIncreasingDateTimeGenerator(DateTime.UtcNow));
            fixture.Customizations.Add(new MethodInvoker(new GreedyConstructorQuery()));
            fixture.Customizations.Add(new ReadingsCollectionsBuilder(_duration, _durationShouldBeExceeded));
        }
    }
}

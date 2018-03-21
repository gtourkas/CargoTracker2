using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Monitoring.TempRangeMonitor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Tests.Monitoring.TempRangeMonitor.Infra
{
    public class DefaultReadingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new DefaultTempCustomization());
        }
    }

    public class RangeReadingCustomization : ICustomization
    {
        private Random _random;

        public int _from;

        public int _till;

        public RangeReadingCustomization(int from, int till)
        {
            _random = new Random();
            _from = from;
            _till = till;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customize(new RangeTempCustomization(_from, _till));
        }
    }
}

using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.Xunit2;
using Domain.Monitoring.TempRangeMonitor;

namespace Domain.Tests.Monitoring.TempRangeMonitor.Infra
{
    public class DefaultSpecificationCustomization : ICustomization
    {
        private Random _random;

        public DefaultSpecificationCustomization()
        {
            _random = new Random();
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customize(new DefaultRangeCustomization());
            fixture.Register(() => new Duration(TimeSpan.FromMinutes(_random.Next(5, 20))));
        }
    }

}

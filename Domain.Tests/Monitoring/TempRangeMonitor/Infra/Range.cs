using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Domain.Monitoring.TempRangeMonitor;

namespace Domain.Tests.Monitoring.TempRangeMonitor.Infra
{
    public class DefaultRangeCustomization : ICustomization
    {
        private Random _random;

        public DefaultRangeCustomization()
        {
            _random = new Random();
        }

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new Range(new Temp(_random.Next(-15, 0)), new Temp(_random.Next(0, 15))));
        }
    }

}

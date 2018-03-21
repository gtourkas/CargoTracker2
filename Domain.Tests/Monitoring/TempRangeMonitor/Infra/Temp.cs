using AutoFixture;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Domain.Monitoring.TempRangeMonitor;
using System.Linq;
using System.Reflection;

namespace Domain.Tests.Monitoring.TempRangeMonitor.Infra
{
    public class DefaultTempCustomization : ICustomization
    {
        private Random _random;

        public DefaultTempCustomization()
        {
            _random = new Random();
        }

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new Temp(_random.Next(Temp.MinValue, Temp.MaxValue)));
        }
    }

    public class RangeTempCustomization : ICustomization
    {
        private Random _random;

        public int _from;

        public int _till;

        public RangeTempCustomization(int from, int till)
        {
            _random = new Random();
            _from = from;
            _till = till;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new Temp(_random.Next(_from, _till)));
        }
    }


}

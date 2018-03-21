using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Monitoring.WeightShiftMonitor;

namespace Domain.Tests.Monitoring.WeightShiftMonitor.Infra
{
    public class DefaultPercentageCustomization : ICustomization
    {
        private Random _random = new Random();

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new Percentage(_random.Next(1, 100)));
        }
    }

    public class RangePercentageCustomization : ICustomization
    {
        private Random _random = new Random();

        private int _from;

        private int _till;

        public RangePercentageCustomization(int from, int till)
        {
            _from = from;
            _till = till;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new Percentage(_random.Next(_from, _till)));
        }
    }
}

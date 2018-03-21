using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Domain.Monitoring.WeightShiftMonitor;

namespace Domain.Tests.Monitoring.WeightShiftMonitor.Infra
{
    public class DefaultSpecificationCustomization : ICustomization
    {

        public void Customize(IFixture fixture)
        {
            fixture.Customize(new RangePercentageCustomization(25,50));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;

namespace Domain.Tests.Monitoring.TempRangeMonitor.Infra
{
    public class DefaultMonitorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new DefaultSpecificationCustomization());
        }
    }
}

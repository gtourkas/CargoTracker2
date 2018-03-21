using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using AutoFixture;
using Domain.Monitoring.WeightShiftMonitor;

namespace Domain.Tests.Monitoring.WeightShiftMonitor.Infra
{
    public class DefaultMonitorCustomization : ICustomization
    {

        public void Customize(IFixture fixture)
        {
            fixture.Customize(new DefaultSpecificationCustomization());
        }


    }
}

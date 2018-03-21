using System;
using System.Collections.Generic;
using System.Text;
using Domain.Monitoring.WeightShiftMonitor;
using Xunit;

namespace Domain.Tests.Monitoring.WeightShiftMonitor
{
    public class WeightUnitTest
    {
        [Fact]
        public void CalcPercDiff__WeightGivenGreaterThan()
        {
            // ARRANGE 
            var weight = new Weight(75);
            var otherWeight = new Weight(100);
            
            // ACT
            var perc = weight.CalcPercDiff(otherWeight);

            // ASSERT
            Assert.Equal(33, perc.Value);
        }
    }
}

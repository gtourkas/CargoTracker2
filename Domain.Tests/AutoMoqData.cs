using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0618

namespace Domain.Tests
{

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }

    public class AutoConfiguredMoqDataAttribute : AutoDataAttribute
    {
        public AutoConfiguredMoqDataAttribute()
            : base(new Fixture().Customize(new AutoConfiguredMoqCustomization()))
        {
        }
    }
}

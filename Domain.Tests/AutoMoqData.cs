using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

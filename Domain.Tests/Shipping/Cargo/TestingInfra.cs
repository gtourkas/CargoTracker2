using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Domain.Tests.Shipping.Cargo
{
    public class LegBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var load = new DateTime(2017, 1, 1, 0, 0, 0);
            var paramInfo = request as ParameterInfo;
            if (paramInfo != null
                && paramInfo.Name == "loadTime")
            { return load; }

            if (paramInfo != null
                && paramInfo.Name == "unloadTime")
            { return load.AddSeconds(1); }

            return new NoSpecimen();
        }
    }

    public class AutoCargoCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new LegBuilder());
        }
    }

    public class AutoCargoDataAttribute : AutoDataAttribute
    {
        public AutoCargoDataAttribute()
            : base(new Fixture().Customize(new AutoCargoCustomization()))
        {
            // foo
        }
    }

    public class InlineAutoCargoDataAttribute : CompositeDataAttribute
    {
        public InlineAutoCargoDataAttribute(params object[] values)
            : base(new DataAttribute[] {
            new InlineDataAttribute(values), new AutoCargoDataAttribute() })
        {
            // foo
        }
    }


    public static class RecreateWithMethods {

        public static HandlingEvent RecreateWith(this HandlingEvent @event, HandlingType type)
        {
            return new HandlingEvent(@event.TrackingId
                , type
                , @event.Location
                , @event.Voyage
                , @event.Completed
                , @event.Registered);
        }
        public static HandlingEvent RecreateWith(this HandlingEvent @event, UnLocode location)
        {
            return new HandlingEvent(@event.TrackingId
                , @event.Type
                , location
                , @event.Voyage
                , @event.Completed
                , @event.Registered);
        }

        public static HandlingEvent RecreateWith(this HandlingEvent @event, VoyageNumber voyage)
        {
            return new HandlingEvent(@event.TrackingId
                , @event.Type
                , @event.Location
                , voyage
                , @event.Completed
                , @event.Registered);
        }
    }
}

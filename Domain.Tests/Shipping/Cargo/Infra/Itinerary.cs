using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;
using Xunit.Sdk;
using Domain.Shipping.Cargo;
using System.Reflection;

namespace Domain.Tests.Shipping.Cargo.Infra
{

    public class DefaultItineraryCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new DefaultLegCustomization());
        }
    }

    public class ItineraryBuilder : ISpecimenBuilder
    {
        private readonly IList<Leg> _legs;

        public ItineraryBuilder(IList<Leg> legs)
        {
            _legs = legs;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var paramInfo = request as ParameterInfo;
            if (paramInfo != null
                && (paramInfo.Name == "legs"))
            {
                return _legs;
            }

            return new NoSpecimen();
        }

    }

}

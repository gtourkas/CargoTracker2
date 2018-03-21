using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Shipping.Location;

namespace Domain.Tests.Shipping.Cargo.Infra
{

    public class DefaultLegCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new StrictlyMonotonicallyIncreasingDateTimeGenerator(DateTime.UtcNow));
        }
    }

    public class LegCollectionBuilder : ISpecimenBuilder
    {
        private readonly IEnumerator<UnLocode> _locsEnum;

        public LegCollectionBuilder(IEnumerable<UnLocode> locations)
        {
            _locsEnum = locations.GetEnumerator();
        }

        public object Create(object request, ISpecimenContext context)
        {
            var paramInfo = request as ParameterInfo;
            if (paramInfo != null
                && (paramInfo.Name == "loadLocation" || paramInfo.Name == "unloadLocation"))
            {
                UnLocode nextLoc = null;
                if (_locsEnum.MoveNext())
                    nextLoc = _locsEnum.Current;
                
                if (nextLoc == null)
                    nextLoc = context.Create<UnLocode>();

                return nextLoc;
            }

            return new NoSpecimen();
        }

    }

}

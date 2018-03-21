using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoFixture;
using Domain.Shipping.Cargo;
using AutoFixture.Kernel;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;

namespace Domain.Tests.Shipping.Cargo.Infra
{
    public class HandlingEventBuilder : ISpecimenBuilder
    {
        private bool _typeSet = false;

        public HandlingType Type { get; private set; }

        public HandlingEventBuilder With(HandlingType type)
        {
            _typeSet = true;
            Type = type;
            return this;
        }

        private bool _locationSet = false;

        public UnLocode Location { get; private set; }

        public HandlingEventBuilder With(UnLocode location)
        {
            _locationSet = true;
            Location = location;
            return this;
        }

        private bool _voyageSet = false;

        public VoyageNumber Voyage { get; private set; }

        public HandlingEventBuilder With(VoyageNumber voyage)
        {
            _voyageSet = true;
            Voyage = voyage;
            return this;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var paramInfo = request as ParameterInfo;
            if (paramInfo != null
                && paramInfo.Name == "type")
            { return _typeSet ? Type : context.Create<HandlingType>(); }

            if (paramInfo != null
                && paramInfo.Name == "location")
            { return _locationSet ? Location : context.Create<UnLocode>(); }

            if (paramInfo != null
                && paramInfo.Name == "voyage")
            { return _voyageSet ? Voyage : context.Create<VoyageNumber>(); }

            return new NoSpecimen();
        }
    }
}

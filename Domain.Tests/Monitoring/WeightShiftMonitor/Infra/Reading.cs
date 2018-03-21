using AutoFixture;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.Kernel;
using Domain.Monitoring.WeightShiftMonitor;
using System.Reflection;

namespace Domain.Tests.Monitoring.WeightShiftMonitor.Infra
{
    public class DefaultReadingCustomization : ICustomization
    {
        private int[] _weights;

        private int _percentage;

        private bool _shouldExceedPercentage;

        public DefaultReadingCustomization(int[] weights, int percentage, bool shouldExceedPercentage)
        {
            _weights = weights;
            _percentage = percentage;
            _shouldExceedPercentage = shouldExceedPercentage;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new DefaultReadingSpecimenBuilder(_weights, _percentage, _shouldExceedPercentage));
        }
    }

    public class DefaultReadingSpecimenBuilder : ISpecimenBuilder
    {
        private Random _random;

        private int[] _weights;

        private int _percentage;

        private bool _shouldExceedPercentage;

        public DefaultReadingSpecimenBuilder(int[] weights, int percentage, bool shouldExceedPercentage)
        {
            _weights = weights;
            _percentage = percentage;
            _shouldExceedPercentage = shouldExceedPercentage;

            _random = new Random();
        }

        public object Create(object request, ISpecimenContext context)
        {
            var paramInfo = request as ParameterInfo;
            if (paramInfo != null
                && paramInfo.Name == "frontSide")
            {
                return _createFrom(_weights[(int)Directions.Front] );
            }

            if (paramInfo != null
                && paramInfo.Name == "rearSide")
            {
                return _createFrom(_weights[(int)Directions.Rear]);
            }

            if (paramInfo != null
                && paramInfo.Name == "leftSide")
            {
                return _createFrom(_weights[(int)Directions.Left]);
            }

            if (paramInfo != null
                && paramInfo.Name == "rightSide")
            {
                return _createFrom(_weights[(int)Directions.Right]);
            }

            return new NoSpecimen();
        }

        private Weight _createFrom(int weight)
        {
            var offset = weight * (_percentage * 1.0) / 100;

            var flooredOffset = (int)Math.Floor(offset);

            var ceilingedOffset = (int)Math.Ceiling(offset);

            return new Weight(_shouldExceedPercentage ? _random.Next(weight + ceilingedOffset, Int32.MaxValue) : _random.Next(weight - flooredOffset, weight + flooredOffset));
        }
    }
}



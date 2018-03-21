using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoFixture;
using AutoFixture.Xunit2;
using Domain.Monitoring.TempRangeMonitor;
using Domain.Tests.Shipping;
using Xunit;
using Xunit.Sdk;

namespace Domain.Tests.Monitoring.TempRangeMonitor
{
    public enum GranularityOptions
    {
        Hour,
        Half,
        Quarter,
        Minute
    }

    public class RandomDateTimeSequenceGenerator : ISpecimenBuilder
    {

        public GranularityOptions Granularity { get; set; }

        private readonly object _syncRoot;

        private readonly Random _random;

        private DateTime _start;

        public RandomDateTimeSequenceGenerator(GranularityOptions granularity)
        {
            Granularity = granularity;

            _syncRoot = new object();
            _random = new Random();
            var now = DateTime.UtcNow;
            _start = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, now.Minute), DateTimeKind.Utc);
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return _isNotDateTimeRequest(request)
                ? new NoSpecimen()
                : this._createNextRandom();
        }

        private bool _isNotDateTimeRequest(object request)
        {
            return !typeof(DateTime).GetTypeInfo().IsAssignableFrom(request as Type);
        }

        private object _createNextRandom()
        {
            lock (this._syncRoot)
            {
                var minutes = 0;

                switch (Granularity)
                {
                    case GranularityOptions.Hour:
                        minutes = _random.Next(1, 24) * 60;
                        break;
                    case GranularityOptions.Half:
                        minutes = _random.Next(1, 2) * 30;
                        break;
                    case GranularityOptions.Quarter:
                        minutes = _random.Next(1, 4) * 15;
                        break;
                    case GranularityOptions.Minute:
                        minutes = _random.Next(1, 60);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _start = _start.AddMinutes(minutes);

                return _start;
            }
        }
    }



    public class TempBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            return new Temp(new Random().Next(-60, 60));
        }
    }

    public class AutoTempCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Temp>(_ => _.FromFactory(new TempBuilder()));
        }
    }

    public class AutoTempDataAttribute : AutoDataAttribute
    {
        public AutoTempDataAttribute()
            : base(() => new Fixture().Customize(new AutoTempCustomization()))
        {
            // foo
        }
    }

    public class AutoReadingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Temp>(_ => _.FromFactory(new TempBuilder()));
            fixture.Customizations.Add(new RandomDateTimeSequenceGenerator(GranularityOptions.Minute));
        }
    }

    public class AutoReadingDataAttribute : AutoDataAttribute
    {
        public AutoReadingDataAttribute()
            : base(() => new Fixture().Customize(new AutoReadingCustomization()))
        {
            // foo
        }
    }

    public class RangeBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var from = context.Create<Temp>();

            return new Range(from, new Temp(from.Value+1));
        }

    }

    public class AutoMonitorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Temp>(_ => _.FromFactory(new TempBuilder()));
            fixture.Customize<Range>(_ => _.FromFactory(new RangeBuilder()));
        }
    }

    public class AutoMonitorDataAttribute : AutoDataAttribute
    {
        public AutoMonitorDataAttribute()
            : base(() => new Fixture().Customize(new AutoMonitorCustomization()))
        {
            // foo
        }
    }

    public class InlineAutoMonitorDataAttribute : CompositeDataAttribute
    {
        public InlineAutoMonitorDataAttribute(params object[] values)
            : base(new DataAttribute[] {
            new InlineDataAttribute(values), new AutoMonitorDataAttribute() })
        {
            // foo
        }
    }



}

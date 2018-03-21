using System;
using System.Collections.Generic;
using Domain.Monitoring.TempRangeMonitor.Events;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class Monitor : BaseAggregateRoot
    {
        public ContainerId ContainerId { get; }

        public Specification Specification { get;  }

        public Reading LastReading { get; private set; }

        public ReadingsCollection ConsecOutOfRangeReadings { get; }

        public bool AlarmStarted { get; private set; }

        public Monitor(ContainerId containerId
            , Specification spec)
        {
            ContainerId = containerId ?? throw new ArgumentNullException(nameof(containerId));
            Specification = spec ?? throw new ArgumentNullException(nameof(spec));

            ConsecOutOfRangeReadings = new ReadingsCollection();
        }

        // rehydration ctor
        public Monitor(ContainerId contanerId
            , Specification specification
            , Reading lastReading
            , ReadingsCollection consecOutOfRangeReadings
            , bool alarmStarted)
        {
            ContainerId = contanerId;
            Specification = specification;
            LastReading = lastReading;
            ConsecOutOfRangeReadings = consecOutOfRangeReadings;
            AlarmStarted = alarmStarted;
        }

        public void Check(Reading reading)
        {
            // out of range
            if (Specification.Range.IsOut(reading.Temperature))
            {
                ConsecOutOfRangeReadings.Add(reading);

                // start the alarm if the duration that the readings can be out of range is exceeded
                if (ConsecOutOfRangeReadings.TotalDuration.GreaterThan(this.Specification.Duration)
                    &&
                    !AlarmStarted
                    )
                {
                    AlarmStarted = true;

                    this.Events.Add(new AlarmStarted(ContainerId, reading));
                }
            }
            // in range
            else
            {
                // stop the alarm, if started
                if (AlarmStarted)
                {
                    AlarmStarted = false;

                    this.Events.Add(new AlarmStopped(ContainerId, reading));

                    ConsecOutOfRangeReadings.Clear();
                }
            }

            LastReading = reading;
        }
    }
}

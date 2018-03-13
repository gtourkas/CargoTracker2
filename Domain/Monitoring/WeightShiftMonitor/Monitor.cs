using Domain.Monitoring.WeightShiftMonitor.Events;
using System;

namespace Domain.Monitoring.WeightShiftMonitor
{
    public class Monitor : BaseAggregateRoot
    {
        public ContainerId ContainerId { get; }

        public Specification Specification { get; }

        public Reading LastReading { get; private set; }

        public bool AlarmStarted { get; private set; }

        public Monitor(ContainerId containerId, Specification spec)
        {
            ContainerId = containerId ?? throw new ArgumentNullException(nameof(containerId));
            Specification = spec ?? throw new ArgumentNullException(nameof(spec));
        }

        // rehydration ctor
        public Monitor(ContainerId containerId, Specification spec, Reading lastReading, bool alarmStarted)
        {
            ContainerId = containerId;
            Specification = spec;
            LastReading = lastReading;
            AlarmStarted = alarmStarted;
        }

        public void Check(Reading reading)
        {
            if (LastReading == null)
                return;

            var shift = LastReading.CalcShift(reading);

            // greater than spec perc, start the alarm if not already
            if (shift.PercOfLargestShift.GreaterThan(Specification.Percentage)  && !AlarmStarted)
            {
                AlarmStarted = true;

                this.Events.Add(new AlarmStarted(ContainerId, LastReading, shift.PercOfLargestShift, shift.DirOfLargestShift));
            }
            // less than spec perc
            else
            {
                // stop the alarm, if started
                if (AlarmStarted)
                {
                    AlarmStarted = false;

                    this.Events.Add(new AlarmStopped(ContainerId, LastReading));
                }
            }

            LastReading = reading;
        }

    }
}

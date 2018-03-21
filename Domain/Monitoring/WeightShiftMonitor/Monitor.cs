using Domain.Monitoring.WeightShiftMonitor.Events;
using System;

namespace Domain.Monitoring.WeightShiftMonitor
{
    public class Monitor : BaseAggregateRoot
    {
        public ContainerId ContainerId { get; }

        public Specification Specification { get; }

        public Reading InitialReading { get; }

        public bool AlarmStarted { get; private set; }

        public Monitor(ContainerId containerId, Specification spec, Reading initialReading)
        {
            ContainerId = containerId ?? throw new ArgumentNullException(nameof(containerId));
            Specification = spec ?? throw new ArgumentNullException(nameof(spec));
            InitialReading = initialReading ?? throw new ArgumentNullException(nameof(initialReading));
        }

        // rehydration ctor
        public Monitor(ContainerId containerId, Specification spec, Reading initialReading, bool alarmStarted)
        {
            ContainerId = containerId;
            Specification = spec;
            InitialReading = initialReading;
            AlarmStarted = alarmStarted;
        }

        public void Check(Reading reading)
        {
            var shift = InitialReading.CalcShift(reading);

            // greater than spec perc, start the alarm if not already
            if (shift.PercOfLargestShift.GreaterThan(Specification.Percentage))
            {
                if (!AlarmStarted)
                {
                    AlarmStarted = true;

                    this.Events.Add(new AlarmStarted(ContainerId, reading, shift.PercOfLargestShift, shift.DirOfLargestShift));
                }
            }
            // less than spec perc
            else
            {
                // stop the alarm, if started
                if (AlarmStarted)
                {
                    AlarmStarted = false;

                    this.Events.Add(new AlarmStopped(ContainerId, reading));
                }
            }
        }

    }
}

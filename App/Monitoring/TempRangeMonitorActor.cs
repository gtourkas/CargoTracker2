using Akka.Actor;
using Domain.Monitoring;
using System;
using Domain.Monitoring.TempRangeMonitor;

namespace App.Monitoring
{
    public class TempRangeMonitorActor : UntypedActor
    {
        public class TempReadingMessage
        {
            public string ContainerId { get; set; }

            public DateTime Timestamp { get; set; }

            public float Reading { get; set; }
        }


        public IRepo TempRangeMonitorRepo { get; set; }

        public IEventDispatcher EventDispatcher { get; set; }

        public string ContainerId { get; }

        public TempRangeMonitorActor(Domain.Monitoring.TempRangeMonitor.IRepo tempRangeMonitorRepo
            , IEventDispatcher eventDispatcher
            , string containerId)
        {
            TempRangeMonitorRepo = tempRangeMonitorRepo;
            EventDispatcher = eventDispatcher;
            ContainerId = containerId;
        }

        private Monitor _monitor;

        protected override void PreStart()
        {
            // rehydrate the aggregate
            _monitor = TempRangeMonitorRepo.GetAsync(new ContainerId(ContainerId)).Result;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case TempReadingMessage m:

                    if (m.ContainerId != ContainerId)
                        return;

                    // check temp reading
                    _monitor.Check(new Reading(new Temp(m.Reading), m.Timestamp));

                    // persist the aggregate
                    TempRangeMonitorRepo.SaveAsync(_monitor);

                    // emit any events
                    EventDispatcher.DispatchAsync(_monitor.Events).Wait();

                    break;
                default:
                    break;
            }
        }

        public static Props Props(Domain.Monitoring.TempRangeMonitor.IRepo tempRangeMonitorRepo, IEventDispatcher eventDispatcher, string containerId)
        {
            return Akka.Actor.Props.Create(() => new TempRangeMonitorActor(tempRangeMonitorRepo, eventDispatcher, containerId));
        }


    }
}

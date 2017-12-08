using System;

namespace Domain.Shipping.Cargo.Events
{

    public class HandlingEventRegistered : IEvent
    {
        public HandlingEvent HandlingEvent { get; private set; }

        public HandlingEventRegistered(HandlingEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("event");

            HandlingEvent = @event;
        }

    }
}

using System;

namespace Domain.Shipping.Cargo.Events
{

    public class DeliveryStateChanged : IEvent
    {
        public TrackingId TrackingId { get; private set; }

        public Delivery Delivery { get; private set; }

        public DeliveryStateChanged(TrackingId trackingId, Delivery delivery)
        {
            if (trackingId == null)
                throw new ArgumentNullException("trackingId");

            if (delivery == null)
                throw new ArgumentNullException("delivery");

            TrackingId = trackingId;
            Delivery = delivery;
        }

    }
}

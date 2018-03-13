using System;

namespace Domain.Shipping.Cargo
{
    public class TrackingId 
    {

        public TrackingId(System.Guid value)
        {
            Value = value;
        }

        public TrackingId()
        {
            Value = System.Guid.NewGuid();
        }

        public Guid Value
        {
            get;
            private set;
        }


    }
}

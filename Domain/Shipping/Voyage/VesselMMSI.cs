using System;

namespace Domain.Shipping.Voyage
{
    public class VesselMMSI
    {
        public string Value { get; private set; }

        public VesselMMSI(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("value cannot be null or empty");

            Value = value;
        }
    }
}

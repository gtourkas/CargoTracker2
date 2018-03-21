using System;

namespace Domain.Shipping.Voyage
{
    public class VoyageNumber
    {
        public string Value { get; private set; }

        public VoyageNumber(string value)
        {
            Value = value ?? throw new ArgumentNullException("value");
        }

    }
}

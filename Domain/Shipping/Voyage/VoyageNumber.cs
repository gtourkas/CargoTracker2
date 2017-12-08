using System;

namespace Domain.Shipping.Voyage
{
    public class VoyageNumber : IEquatable<VoyageNumber>
    {
        public string Value { get; private set; }

        public VoyageNumber(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Value = value;
        }

        public bool Equals(VoyageNumber other)
        {
            if (other == null)
                return false;

            return Value.Equals(other.Value);
        }
    }
}

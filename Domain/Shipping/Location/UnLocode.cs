using System;
using System.Text.RegularExpressions;

namespace Domain.Shipping.Location
{
    public class UnLocode 
    {
        private static readonly Regex _pattern = new Regex("[a-zA-Z]{2}[a-zA-Z2-9]{3}"
            , RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public UnLocode(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (!_pattern.Match(value).Success)
                throw new ArgumentException("Provided value is not a valid UnLocode", "value");

            Value = value;
        }

        public string Value { get; private set; }

    }
}
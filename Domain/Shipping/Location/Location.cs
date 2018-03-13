using System;

namespace Domain.Shipping.Location
{
    // Root of the Location Aggregate
    public class Location
    {
        public UnLocode UnLocode { get; private set; }

        public string Name { get; private set; }

        public Location(UnLocode unLocode, string name)
        {
            UnLocode = unLocode ?? throw new ArgumentNullException(nameof(unLocode));

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Provided name is not valid", "name");

            Name = name;
        }
    }
}

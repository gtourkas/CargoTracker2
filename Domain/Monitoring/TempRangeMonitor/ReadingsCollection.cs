using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Monitoring.TempRangeMonitor
{
    public class ReadingsCollection
    {
        public ICollection<Reading> Items { get; }

        public Duration TotalDuration { get; private set; }

        public ReadingsCollection() : this(new List<Reading>()) { }

        // rehydration ctor
        public ReadingsCollection(ICollection<Reading> items)
        {
            Items = items;

            TotalDuration = items.Count > 0 ? 
                new Duration(items.Last().Timestamp.Subtract(items.First().Timestamp)) 
                    :
                new Duration(TimeSpan.Zero);        
        }

        public void Add(Reading item)
        {
            Items.Add(item);

            TotalDuration = new Duration(Items.Last().Timestamp.Subtract(Items.First().Timestamp));
        }

        public void Clear()
        {
            Items.Clear();

            TotalDuration = new Duration(TimeSpan.Zero);
        }

    }
}

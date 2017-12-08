using System.Collections.Generic;

namespace Domain
{
    // Base Class for all Aggregate Roots
    public abstract class BaseAggregateRoot
    {
        public IList<IEvent> Events { get; private set; }

        public BaseAggregateRoot()
        {
            Events = new List<IEvent>();
        }

    }
}

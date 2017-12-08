using Domain.Shipping.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Shipping.Voyage;
using System.Collections.ObjectModel;

namespace Domain.Shipping.Cargo
{
    public interface IItinerary
    {
        bool IsExpected(HandlingEvent @event);
        UnLocode FirstLoadLocation { get; }
        UnLocode LastUnloadLocation { get; }
        DateTime FinalArrivalDate { get; }
        VoyageNumber FirstYoyage { get; }
        IReadOnlyCollection<Leg> Legs { get; }
        Leg NextOf(UnLocode location);
        Leg Of(UnLocode location);

    }

    public class Itinerary : IItinerary
    {
        public IReadOnlyCollection<Leg> Legs { get; private set; }

        public Itinerary(IList<Leg> legs)
        {
            if (legs == null)
                throw new ArgumentNullException("legs");
            if (legs.Count == 0)
                throw new ArgumentException("legs cannot be empty");

            Legs = new ReadOnlyCollection<Leg>(legs);
        }

        public UnLocode FirstLoadLocation
        {
            get
            {
                return Legs.First().LoadLocation;
            }
        }

        public VoyageNumber FirstYoyage
        {
            get
            {
                return Legs.First().Voyage;
            }
        }

        public UnLocode LastUnloadLocation
        {
            get
            {
                return Legs.Last().UnloadLocation;
            }
        }

        public DateTime FinalArrivalDate
        {
            get
            {
                return Legs.Last().UnloadTime;
            }
        }

        public Leg NextOf(UnLocode location)
        {
            var next = (Leg)null;
            var en = Legs.GetEnumerator();
            var currentFound = false;
            while (en.MoveNext())
            {
                if (currentFound)
                {
                    next = en.Current;
                    break;
                }
                if (en.Current.LoadLocation.Equals(location) || en.Current.UnloadLocation.Equals(location))
                    currentFound = true;
            }
            return next;
        }

        public Leg Of(UnLocode location)
        {
            return Legs.Where(l => l.UnloadLocation.Equals(location) || l.LoadLocation.Equals(location))
                .FirstOrDefault();
        }

        public bool IsExpected(HandlingEvent @event) {

            if (@event == null)
                return true;

            // receive at the first leg's load location
            switch (@event.Type) { 
                case HandlingType.Receive:
                    return this.FirstLoadLocation.Equals(@event.Location);
                case HandlingType.Load:
                    foreach (var leg in Legs)
                        if (leg.LoadLocation.Equals(@event.Location))
                            return true;
                    return false;
                case HandlingType.Unload:
                    foreach (var leg in Legs)
                        if (leg.UnloadLocation.Equals(@event.Location))
                            return true;
                    return false;
                case HandlingType.Claim:
                case HandlingType.Customs:
                    return this.LastUnloadLocation.Equals(@event.Location);
            }

            return false;
        }

    }
}

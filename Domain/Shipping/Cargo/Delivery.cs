using System;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;

namespace Domain.Shipping.Cargo
{
    public class Delivery 
    {
        public RouteSpecification RouteSpec { get; private set; }

        public Itinerary Itinerary { get; private set; }

        public HandlingEvent LastHandlingEvent { get; private set; }

        public TransportStatus TransportStatus { get; private set; }

        public RoutingStatus RoutingStatus { get; private set; }

        public UnLocode LastKnownLocation { get; private set; }

        public VoyageNumber CurrentVoyage { get; private set; }

        public HandlingActivity NextExpectedHandlingActivity { get; private set; }

        public bool IsUnloadedAtDestination { get; private set; }

        public bool IsMishandled { get; private set; }

        public Delivery(
            RouteSpecification routeSpec,
            Itinerary itinerary,
            HandlingEvent lastHandlingEvent
            )
        {
            RouteSpec = routeSpec ?? throw new ArgumentNullException(nameof(routeSpec));
            Itinerary = itinerary;
            LastHandlingEvent = lastHandlingEvent;

            _calcTransportStatus(lastHandlingEvent);
            _calcLastKnownLocation(lastHandlingEvent);
            _calcCurrentVoyage(lastHandlingEvent);
            _calcNextExpectedHandlingActivity(routeSpec, itinerary, lastHandlingEvent);
            _calcIsUnloadedAtDestination(routeSpec, lastHandlingEvent);
            _calcRoutingStatus(routeSpec, itinerary);
            _calcIsMishandled(itinerary, lastHandlingEvent);
        }

        private void _calcTransportStatus(HandlingEvent @event)
        {
            if (@event == null)
            {
                TransportStatus = TransportStatus.NotReceived;
            }
            else
            {
                switch (@event.Type)
                {
                    case HandlingType.Load:
                        TransportStatus = TransportStatus.OnBoardVessel;
                        return;
                    case HandlingType.Unload:
                    case HandlingType.Receive:
                    case HandlingType.Customs:
                        TransportStatus = TransportStatus.InPort;
                        return;
                    case HandlingType.Claim:
                        TransportStatus = TransportStatus.Claimed;
                        return;
                }
            }
        }

        private void _calcRoutingStatus(RouteSpecification routeSpec, Itinerary itinerary)
        {
            if (itinerary == null)
                RoutingStatus = RoutingStatus.NotRouted;
            else if (routeSpec.IsSatisfiedBy(itinerary))
                RoutingStatus = RoutingStatus.Routed;
            else
                RoutingStatus = RoutingStatus.MisRouted;
        }

        private void _calcLastKnownLocation(HandlingEvent @event)
        {
            if (@event != null)
                LastKnownLocation = @event.Location;
            else
                LastKnownLocation = null;
        }

        private void _calcCurrentVoyage(HandlingEvent @event)
        {
            if (@event != null)
                CurrentVoyage = @event.Voyage;
            else
                CurrentVoyage = null;
        }

        private void _calcNextExpectedHandlingActivity(RouteSpecification routeSpec, Itinerary itinerary, HandlingEvent @event)
        {
            // can't derive next handling activity if there is no itinerary
            if (itinerary == null)
            {
                NextExpectedHandlingActivity = null;
                return;
            }

            // can't derive the next handling activity if the cargo is misrouted
            _calcRoutingStatus(routeSpec, itinerary);
            if (RoutingStatus == RoutingStatus.MisRouted)
            {
                NextExpectedHandlingActivity = null;
                return;
            }

            // no handling event so far; next one should be receive at itinerary's first leg unload location
            if (@event == null)
            {
                NextExpectedHandlingActivity = new HandlingActivity(HandlingType.Receive, itinerary.FirstLoadLocation, null);
                return;
            }

            switch (@event.Type)
            {
                // last handling event = received => next handling activity = load at itinerary's first leg unload location and voyage
                case HandlingType.Receive:

                    NextExpectedHandlingActivity = new HandlingActivity(HandlingType.Load, itinerary.FirstLoadLocation, itinerary.FirstYoyage);

                    return;

                // last handling event = load => next handling activity = unload at itinerary's next leg unload location
                case HandlingType.Load:
                    var leg = itinerary.Of(@event.Location);

                    NextExpectedHandlingActivity = new HandlingActivity(HandlingType.Unload, leg.UnloadLocation, leg.Voyage);

                    return;

                // last handling event = unload => next handling activity = 
                // a. load at itinerary's next leg load location and voyage, if that leg is not the final one
                // b. customs at itinerarys's final unload location
                case HandlingType.Unload:

                    if (@event.Location.Equals(itinerary.LastUnloadLocation))
                    {
                        NextExpectedHandlingActivity =  new HandlingActivity(HandlingType.Customs, itinerary.LastUnloadLocation, null);
                        return;
                    }

                    var nextLeg = itinerary.NextOf(@event.Location);
                    NextExpectedHandlingActivity = new HandlingActivity(HandlingType.Load, nextLeg.LoadLocation, nextLeg.Voyage);
                    return;

                // last handling event = customs => next handling activity = claim
                case HandlingType.Customs:
                    NextExpectedHandlingActivity = new HandlingActivity(HandlingType.Claim, itinerary.LastUnloadLocation, null);
                    return;

                // last handling event = claim => next handling activity = none
                case HandlingType.Claim:
                default:
                    NextExpectedHandlingActivity = null;
                    return;
            }

        }

        private void _calcIsUnloadedAtDestination(RouteSpecification routeSpec, HandlingEvent @event)
        {
            if (@event == null)
                IsUnloadedAtDestination = false;
            else
                IsUnloadedAtDestination = routeSpec.Destination.Equals(@event.Location);
        }

        private void _calcIsMishandled(Itinerary itinerary, HandlingEvent @event)
        {
            if (@event == null || itinerary == null)
                IsMishandled = false;
            else
                IsMishandled = itinerary.IsExpected(@event);
        }

    }
}

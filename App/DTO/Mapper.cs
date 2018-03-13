using Domain.Shipping.Cargo;
using Domain.Shipping.Location;
using Domain.Shipping.Voyage;

namespace App.DTO
{
    public static class Mapper
    {
        public static void Map()
        {
            AutoMapper.Mapper.Initialize(cfg => {

                cfg.CreateMap<DTO.Leg, Domain.Shipping.Cargo.Leg>()
                    .ConstructUsing( o => new Domain.Shipping.Cargo.Leg(
                        new VoyageNumber(o.Voyage)
                        , new UnLocode(o.LoadLocation)
                        , new UnLocode(o.UnloadLocation)
                        , o.LoadTime
                        , o.UnloadTime) );

                cfg.CreateMap<DTO.Itinerary, Domain.Shipping.Cargo.Itinerary>();

                cfg.CreateMap<DTO.RouteSpecification, Domain.Shipping.Cargo.RouteSpecification>()
                    .ConstructUsing( o => new Domain.Shipping.Cargo.RouteSpecification(
                        new UnLocode(o.Origin)
                        , new UnLocode(o.Destination)
                        , o.ArrivalDeadline) ) ;

                cfg.CreateMap<DTO.HandlingEvent, Domain.Shipping.Cargo.HandlingEvent>()
                    .ConstructUsing(o => new Domain.Shipping.Cargo.HandlingEvent(
                        new TrackingId(o.TrackingId)
                        , (HandlingType)o.Type
                        , new UnLocode(o.Location)
                        , new VoyageNumber(o.Voyage)
                        , o.Completed
                        , o.Registered
                        ));

            });
        }

    }

    public static class MappingExtensions
    {

        public static Domain.Shipping.Cargo.Leg ToDomain(this DTO.Leg leg)
        {
            return AutoMapper.Mapper.Map<Domain.Shipping.Cargo.Leg>(leg);
        }

        public static Domain.Shipping.Cargo.Itinerary ToDomain(this DTO.Itinerary itinerary)
        {
            return AutoMapper.Mapper.Map<Domain.Shipping.Cargo.Itinerary>(itinerary);
        }

        public static Domain.Shipping.Cargo.RouteSpecification ToDomain(this DTO.RouteSpecification routeSpec)
        {
            return AutoMapper.Mapper.Map<Domain.Shipping.Cargo.RouteSpecification>(routeSpec);
        }

        public static Domain.Shipping.Cargo.HandlingEvent ToDomain(this DTO.HandlingEvent handlingEvent)
        {
            return AutoMapper.Mapper.Map<Domain.Shipping.Cargo.HandlingEvent>(handlingEvent);
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shipping.DTO
{
    public static class Mapper
    {
        public static void Map()
        {
            AutoMapper.Mapper.Initialize(cfg => {

                cfg.CreateMap<DTO.Leg, Domain.Shipping.Cargo.Leg>()
                    .ConstructUsing( o => new Domain.Shipping.Cargo.Leg(
                        new Domain.Shipping.Voyage.VoyageNumber(o.Voyage)
                        , new Domain.Shipping.Location.UnLocode(o.LoadLocation)
                        , new Domain.Shipping.Location.UnLocode(o.UnloadLocation)
                        , o.LoadTime
                        , o.UnloadTime) );

                cfg.CreateMap<DTO.Itinerary, Domain.Shipping.Cargo.Itinerary>();

                cfg.CreateMap<DTO.RouteSpecification, Domain.Shipping.Cargo.RouteSpecification>()
                    .ConstructUsing( o => new Domain.Shipping.Cargo.RouteSpecification(
                        new Domain.Shipping.Location.UnLocode(o.Origin)
                        , new Domain.Shipping.Location.UnLocode(o.Destination)
                        , o.ArrivalDeadline) ) ;

                cfg.CreateMap<DTO.HandlingEvent, Domain.Shipping.Cargo.HandlingEvent>()
                    .ConstructUsing(o => new Domain.Shipping.Cargo.HandlingEvent(
                        new Domain.Shipping.Cargo.TrackingId(o.TrackingId)
                        , (Domain.Shipping.Cargo.HandlingType)o.Type
                        , new Domain.Shipping.Location.UnLocode(o.Location)
                        , new Domain.Shipping.Voyage.VoyageNumber(o.Voyage)
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

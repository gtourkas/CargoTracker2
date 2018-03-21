using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Monitoring;
using Domain.Monitoring.WeightShiftMonitor;
using Domain_Reading = Domain.Monitoring.WeightShiftMonitor.Reading;
using Domain_Monitor = Domain.Monitoring.WeightShiftMonitor.Monitor;
using Data_Reading = Infra.MongoDB.Monitoring.WeightShiftMonitor.Reading;
using Data_Monitor = Infra.MongoDB.Monitoring.WeightShiftMonitor.Monitor;

namespace Infra.MongoDB.Monitoring.WeightShiftMonitor
{
    public class TypeMappings : Profile
    {
        public TypeMappings()
        {
            CreateMap<Domain_Reading, Data_Reading>()
                .ForMember(d => d.FrontSide, o => o.MapFrom(s => s.FrontSide.Value))
                .ForMember(d => d.RightSide, o => o.MapFrom(s => s.RightSide.Value))
                .ForMember(d => d.RearSide, o => o.MapFrom(s => s.RearSide.Value))
                .ForMember(d => d.LeftSide, o => o.MapFrom(s => s.LeftSide.Value))
                ;

            CreateMap<Data_Reading, Domain_Reading>()
                .ConstructUsing(s => new Domain_Reading(
                    new Weight(s.FrontSide)
                    , new Weight(s.LeftSide)
                    , new Weight(s.RearSide)
                    , new Weight(s.LeftSide)
                    , s.Timestamp))
                ;

            CreateMap<Domain_Monitor, Data_Monitor>()
                ;

            CreateMap<Data_Monitor, Domain_Monitor>()
                .ConstructUsing(s => new Domain_Monitor(
                        new ContainerId(s.ContainerId)
                        , new Specification(new Percentage(s.SpecificationPercentage))
                        , null // TODO
                        )
                )
                .ForMember(d => d.Events, o => o.Ignore())
                ;
        }
    }
}

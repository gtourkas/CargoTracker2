using System;
using System.Linq;
using AutoMapper;
using Domain.Monitoring;
using Domain.Monitoring.TempRangeMonitor;
using Domain_Reading = Domain.Monitoring.TempRangeMonitor.Reading;
using Domain_Monitor = Domain.Monitoring.TempRangeMonitor.Monitor;
using Data_Reading = Infra.MongoDB.Monitoring.TempRangeMonitor.Reading;
using Data_Monitor = Infra.MongoDB.Monitoring.TempRangeMonitor.Monitor;

namespace Infra.MongoDB.Monitoring.TempRangeMonitor
{
    public class TypeMappings : Profile
    {
        public TypeMappings()
        {
            CreateMap<Domain_Reading, Data_Reading>()
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Temperature.Value))
                ;

            CreateMap<Data_Reading, Domain_Reading>()
                .ConstructUsing(s => new Domain_Reading(new Temp(s.Value), s.Timestamp))
                ;

            CreateMap<Domain_Monitor, Data_Monitor>()
                ;

            CreateMap<Data_Monitor, Domain_Monitor>()
                .ConstructUsing(s => new Domain_Monitor(
                        new ContainerId(s.ContainerId)
                        , new Specification(new Range(new Temp(s.SpecificationRangeFrom), new Temp(s.SpecificationRangeTill)), new Duration(TimeSpan.FromSeconds(s.SpecificationDuration)))
                        , new Domain_Reading(new Temp(s.LastReading.Value), s.LastReading.Timestamp)
                        , new ReadingsCollection(s.ConsecOutOfRangeReadings.Select(_ => new Domain_Reading(new Temp(_.Value), _.Timestamp)).ToList())
                        , s.AlarmStarted
                    )
                )
                .ForMember(d => d.Events, o => o.Ignore())
                ;
        }
    }
}

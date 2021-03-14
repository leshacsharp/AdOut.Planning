using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Events;
using AutoMapper;

namespace AdOut.Planning.Core.Mapping
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<AdPointCreatedEvent, AdPoint>();
            CreateMap<AdPointDeletedEvent, AdPoint>();
            CreateMap<PlanAdPoint, PlanAdPointCreatedEvent>();
            CreateMap<Plan, PlanCreatedEvent>();
            CreateMap<TariffCreatedEvent, Tariff>();
            CreateMap<PlanTime, PlanHandledEvent>();
        }
    }
}

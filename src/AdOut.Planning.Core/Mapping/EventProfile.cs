using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Events;
using AutoMapper;

namespace AdOut.Planning.Core.Mapping
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<PlanTime, PlanHandledEvent>();
        }
    }
}

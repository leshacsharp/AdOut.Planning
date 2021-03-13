using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AutoMapper;
using System.Linq;

namespace AdOut.Planning.Core.Mapping
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<PlanTimeDto, PlanTime>()
                .ForMember(x => x.AdPoints, x => x.MapFrom(m => m.AdPoints.Select(ap => ap.Id)))
                .ForMember(x => x.Schedules, x => x.Ignore());

            CreateMap<PlanTimeDto, Plan>()
                .ForMember(x => x.Schedules, x => x.Ignore());
        }
    }
}

using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Dto;
using AutoMapper;
using System.Linq;

namespace AdOut.Planning.Core.Mapping
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<ScheduleDto, ScheduleTime>()
                .ForMember(x => x.ScheduleStartTime, x => x.MapFrom(m => m.StartTime))
                .ForMember(x => x.ScheduleEndTime, x => x.MapFrom(m => m.EndTime))
                .ForMember(x => x.ScheduleType, x => x.MapFrom(m => m.Type))
                .ForMember(x => x.ScheduleDayOfWeek, x => x.MapFrom(m => m.DayOfWeek))
                .ForMember(x => x.ScheduleDate, x => x.MapFrom(m => m.Date))
                .ForMember(x => x.AdBreakTime, x => x.MapFrom(m => m.BreakTime))
                .ForMember(x => x.AdPlayTime, x => x.MapFrom(m => m.PlayTime));

            CreateMap<ScheduleModel, ScheduleTime>()
                .ForMember(x => x.AdBreakTime, x => x.MapFrom(m => m.BreakTime))
                .ForMember(x => x.AdPlayTime, x => x.MapFrom(m => m.PlayTime));

            CreateMap<PlanTimeLine, ScheduleTime>()
                .ForMember(x => x.PlanStartDateTime, x => x.MapFrom(m => m.StartDateTime))
                .ForMember(x => x.PlanEndDateTime, x => x.MapFrom(m => m.EndDateTime))
                .ForMember(x => x.AdPointsDaysOff, x => x.MapFrom(m => m.AdPointsDaysOff));

            CreateMap<ScheduleValidation, ScheduleTime>()
                .ForMember(x => x.AdPointsDaysOff, x => x.MapFrom(m => m.AdPoints.SelectMany(ap => ap.DaysOff)));

            CreateMap<PlanExtensionValidation, ScheduleTime>()
                .ForMember(x => x.PlanStartDateTime, x => x.MapFrom(m => m.StartDateTime))
                .ForMember(x => x.PlanEndDateTime, x => x.MapFrom(m => m.EndDateTime))
                .ForMember(x => x.AdPointsDaysOff, x => x.MapFrom(m => m.AdPoints.SelectMany(ap => ap.DaysOff)));

            CreateMap<PlanPriceDto, ScheduleTime>()
                .ForMember(x => x.PlanStartDateTime, x => x.MapFrom(m => m.StartDateTime))
                .ForMember(x => x.PlanEndDateTime, x => x.MapFrom(m => m.EndDateTime))
                .ForMember(x => x.AdPointsDaysOff, x => x.MapFrom(m => m.AdPoints.SelectMany(ap => ap.DaysOff)));
        }
    }
}

using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Classes;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(IDatabaseContext context)
            : base(context)
        {
        }

        public Task<List<ScheduleDto>> GetByPlanAsync(string planId)
        {
            var query = Context.Schedules.Where(s => s.PlanId == planId)
                               .Select(s => new ScheduleDto()
                               {
                                   StartTime = s.StartTime,
                                   EndTime = s.EndTime,
                                   BreakTime = s.BreakTime,
                                   PlayTime = s.PlayTime,
                                   Date = s.Date,
                                   DayOfWeek = s.DayOfWeek
                               });

            return query.ToListAsync();
        }
  
        public Task<ScheduleTime> GetScheduleTimeAsync(string scheduleId)
        {
            var query = Context.Schedules.Where(s => s.Id == scheduleId)
                               .Select(s => new ScheduleTime()
                               {
                                   PlanType = s.Plan.Type,
                                   PlanStartDateTime = s.Plan.StartDateTime,
                                   PlanEndDateTime = s.Plan.EndDateTime,
                                   ScheduleStartTime = s.StartTime,
                                   ScheduleEndTime = s.EndTime,
                                   ScheduleDayOfWeek = s.DayOfWeek,
                                   ScheduleDate = s.Date,
                                   AdPlayTime = s.PlayTime,
                                   AdBreakTime = s.BreakTime,
                                   AdPointsDaysOff = s.Plan.PlanAdPoints
                                         .SelectMany(pap => pap.AdPoint.DaysOff)
                                         .Select(doof => doof.DayOfWeek)
                               });

            return query.SingleOrDefaultAsync();
        }
    }
}

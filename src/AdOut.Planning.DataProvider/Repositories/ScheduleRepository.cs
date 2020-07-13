using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AdOut.Planning.Model.Dto;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(IDatabaseContext context)
            : base(context)
        {
        }

        public Task<List<ScheduleDto>> GetByPlanAsync(int planId)
        {
            var query = from s in Context.Schedules
                        where s.PlanId == planId
                        select new ScheduleDto()
                        {
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            BreakTime = s.BreakTime,
                            Date = s.Date,
                            DayOfWeek = s.DayOfWeek
                        };

            return query.ToListAsync();
        }

        //todo: need to think about AdsTimePlaying and BreakTime (i would like to see these properties in single entity)
        public Task<AdScheduleTime> GetScheduleInfo(int scheduleId)
        {
            var query = from s in Context.Schedules
                        join p in Context.Plans on s.PlanId equals p.Id
                        where s.Id == scheduleId
                        select new AdScheduleTime()
                        {
                            PlanStartDateTime = p.StartDateTime,
                            PlanEndDateTime = p.EndDateTime,
                            ScheduleStartTime = s.StartTime,
                            ScheduleEndTime = s.EndTime,
                            ScheduleDayOfWeek = s.DayOfWeek,
                            ScheduleDate = s.Date,
                            AdPlayTime = p.AdsTimePlaying,
                            AdBreakTime = s.BreakTime
                        };

            return query.SingleOrDefaultAsync();
        }

        public Task<Schedule> GetByIdAsync(int scheduleId)
        {
            return Context.Schedules.SingleOrDefaultAsync(p => p.Id == scheduleId);
        }
    }
}

using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdOut.Planning.Model.Dto;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(IDatabaseContext context)
            : base(context)
        {
        }
  
        public Task<ScheduleTime> GetScheduleTimeAsync(string scheduleId)
        {
            var query = Context.Schedules.Where(s => s.Id == scheduleId)
                                         .Select(s => new ScheduleTime()
                                         {
                                             PlanStartDateTime = s.Plan.StartDateTime,
                                             PlanEndDateTime = s.Plan.EndDateTime,
                                             ScheduleType = s.Type,
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

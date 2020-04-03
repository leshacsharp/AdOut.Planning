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

        public Task<Schedule> GetByIdAsync(int scheduleId)
        {
            return Context.Schedules.SingleOrDefaultAsync(p => p.Id == scheduleId);
        }      
    }
}

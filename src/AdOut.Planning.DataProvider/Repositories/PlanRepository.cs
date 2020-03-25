using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public async Task<List<AdPointPlanDto>> GetByAdPoint(int adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var query = from pap in Context.PlanAdPoints

                        join p in Context.Plans on pap.PlanId equals p.Id
                        join s in Context.Schedules on p.Id equals s.PlanId

                        where pap.AdPointId == adPointId && p.StartDateTime >= dateFrom && p.EndDateTime <= dateTo

                        select new
                        {
                            p.Id,
                            p.AdsTimePlaying,

                            Schedule = new ScheduleDto()
                            {
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                BreakTime = s.BreakTime,
                                DayOfWeek = s.DayOfWeek,
                                Date = s.Date
                            }
                        };

            var plans = await query.ToListAsync();

            var result = from p in plans
                         group p by new { p.Id, p.AdsTimePlaying }
                         into pGroup

                         select new AdPointPlanDto()
                         {
                             Id = pGroup.Key.Id,
                             AdsTimePlaying = pGroup.Key.AdsTimePlaying,
                             Schedules = pGroup.Select(p => p.Schedule)
                         };

            return result.ToList();
        }

        public Task<List<int>> GetAdPointsIds(int plaId)
        {
            var query = from pap in Context.PlanAdPoints
                        where pap.PlanId == plaId
                        select pap.AdPointId;

            return query.ToListAsync();
        }

        public Task<Plan> GetByIdAsync(int planId)
        {
            var query = from p in Context.Plans
                        where p.Id == planId
                        select p;

            return query.SingleOrDefaultAsync();
        }
    }
}

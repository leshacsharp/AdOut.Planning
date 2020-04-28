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

        public async Task<List<AdPointPlanDto>> GetByAdPoints(int[] adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var query = from pap in Context.PlanAdPoints.Where(pap => adPointId.Contains(pap.AdPointId))

                        join p in Context.Plans on pap.PlanId equals p.Id
                        join s in Context.Schedules on p.Id equals s.PlanId

                        where p.StartDateTime >= dateFrom && p.EndDateTime <= dateTo

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

        public async Task<PlanDto> GetDtoByIdAsync(int planId)
        {
            var query = from p in Context.Plans

                        join s in Context.Schedules on p.Id equals s.PlanId into sJoin
                        from s in sJoin.DefaultIfEmpty()

                        join pa in Context.PlanAds on p.Id equals pa.PlanId into paJoin
                        from pa in paJoin.DefaultIfEmpty()

                        join a in Context.Ads on pa.AdId equals a.Id into aJoin
                        from a in aJoin.DefaultIfEmpty()

                        join pap in Context.PlanAdPoints on p.Id equals pap.PlanId into papJoin
                        from pap in papJoin.DefaultIfEmpty()

                        join ap in Context.AdPoints on pap.AdPointId equals ap.Id into apJoin
                        from ap in apJoin.DefaultIfEmpty()

                        where p.Id == planId

                        select new
                        {
                            p.Id,
                            p.Title,
                            p.Type,
                            p.Status,
                            p.StartDateTime,
                            p.EndDateTime,
                            p.AdsTimePlaying,

                            Schedule = s != null ? new ScheduleDto()
                            {
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                BreakTime = s.BreakTime,
                                Date = s.Date,
                                DayOfWeek = s.DayOfWeek
                            } : null,

                            Ad = a != null ? new AdListDto()
                            {
                                Id = a.Id,
                                Title = a.Title,
                                Status = a.Status,
                                ContentType = a.ContentType,
                                PreviewPath = a.PreviewPath
                            } : null,

                            AdPoint = ap != null ? new AdPointDto()
                            {
                                Id = ap.Id,
                                Location = ap.Location
                            } : null
                        };

            var planItems = await query.ToListAsync();
            var plan = planItems.SingleOrDefault();

            var result = plan != null ? new PlanDto()
            {
                Title = plan.Title,
                Type = plan.Type,
                Status = plan.Status,
                StartDateTime = plan.StartDateTime,
                EndDateTime = plan.EndDateTime,
                AdsTimePlaying = plan.AdsTimePlaying,

                Schedules = planItems.Where(p => p.Schedule != null).Select(p => p.Schedule),
                Ads = planItems.Where(p => p.Ad != null).Select(p => p.Ad),
                AdPoints = planItems.Where(p => p.AdPoint != null).Select(p => p.AdPoint)
            } : null;

            return result;
        }

        public Task<Plan> GetByIdAsync(int planId)
        {
            return Context.Plans.SingleOrDefaultAsync(p => p.Id == planId);
        } 
    }
}

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
    public class AdPointRepository : BaseRepository<AdPoint>, IAdPointRepository
    {
        public AdPointRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<AdPoint> GetByIdAsync(string adPointId)
        {
            return Context.AdPoints.SingleOrDefaultAsync(ap => ap.Id == adPointId);
        }

        public async Task<List<AdPointValidation>> GetAdPointsValidationAsync(string adPointsPlanId, DateTime planStart, DateTime planEnd)
        {               
            var query = from papForAdPoints in Context.PlanAdPoints.Where(pap => pap.PlanId == adPointsPlanId)
                        join ap in Context.AdPoints on papForAdPoints.AdPointId equals ap.Id

                        join apd in Context.AdPointDaysOff on ap.Id equals apd.AdPointId into adPointDaysOffJoin
                        from apd in adPointDaysOffJoin.DefaultIfEmpty()

                        join d in Context.DaysOff on apd.DayOffId equals d.Id into daysOffJoin
                        from d in daysOffJoin.DefaultIfEmpty()

                        join papForPlans in Context.PlanAdPoints on ap.Id equals papForPlans.AdPointId into pap2Join
                        from papForPlans in pap2Join.DefaultIfEmpty()

                        join p in Context.Plans on papForPlans.PlanId equals p.Id into plansJoin
                        from p in plansJoin.DefaultIfEmpty()

                        where p.StartDateTime >= planStart && p.EndDateTime <= planEnd

                        select new
                        {
                            ap.Id,
                            ap.Location,
                            ap.StartWorkingTime,
                            ap.EndWorkingTime,

                            DaysOff = d != null ? d.DayOfWeek : (DayOfWeek?)null,
                            Schedules = p != null ? p.Schedules.Where(s => s.Date == null || s.Date <= planEnd)
                                                     .Select(s => new ScheduleDto()
                                                     {
                                                         StartTime = s.StartTime,
                                                         EndTime = s.EndTime,
                                                         BreakTime = s.BreakTime,
                                                         PlayTime = s.PlayTime,
                                                         Date = s.Date,
                                                         DayOfWeek = s.DayOfWeek
                                                     }) : null
                        };

            var adPointValidations = await query.ToListAsync();

            var result = from apv in adPointValidations
                         group apv by new { apv.Id, apv.Location, apv.StartWorkingTime, apv.EndWorkingTime }
                         into apvGroup

                         select new AdPointValidation()
                         {
                             Location = apvGroup.Key.Location,
                             StartWorkingTime = apvGroup.Key.StartWorkingTime,
                             EndWorkingTime = apvGroup.Key.EndWorkingTime,

                             Schedules = apvGroup.Where(apv => apv.Schedules != null).SelectMany(apv => apv.Schedules),
                             DaysOff = apvGroup.Where(apv => apv.DaysOff != null).Select(apv => apv.DaysOff.Value)
                         };

            return result.ToList();
        }
    }
}

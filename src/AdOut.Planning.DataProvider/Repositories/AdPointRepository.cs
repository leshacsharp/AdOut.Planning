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

        public Task<AdPoint> GetByIdAsync(int adPointId)
        {
            return Context.AdPoints.SingleOrDefaultAsync(ap => ap.Id == adPointId);
        }

        public async Task<List<AdPointValidation>> GetAdPointsValidationAsync(int[] adPointIds, DateTime planStart, DateTime planEnd)
        {
            var query = from ap in Context.AdPoints.Where(ap => adPointIds.Contains(ap.Id))

                        join pap in Context.PlanAdPoints on ap.Id equals pap.AdPointId into planAdPointsJoin
                        from pap in planAdPointsJoin.DefaultIfEmpty()

                        join p in Context.Plans on pap.PlanId equals p.Id into plansJoin
                        from p in plansJoin.DefaultIfEmpty()

                        join apd in Context.AdPointDaysOff on ap.Id equals apd.AdPointId into adPointDaysOffJoin
                        from apd in adPointDaysOffJoin.DefaultIfEmpty()

                        join d in Context.DaysOff on apd.DayOffId equals d.Id into daysOffJoin
                        from d in daysOffJoin.DefaultIfEmpty()

                        where p.StartDateTime <= planStart && p.EndDateTime >= planStart ||
                              p.StartDateTime <= planEnd && p.EndDateTime >= planEnd ||
                              p.StartDateTime >= planStart && p.EndDateTime <= planEnd

                        select new
                        {
                            ap.Id,
                            ap.Location,
                            ap.StartWorkingTime,
                            ap.EndWorkingTime,

                            DaysOff = d != null ? d.DayOfWeek : (DayOfWeek?)null,
                            Plan = p != null ? new PlanValidation()
                            {
                                Type = p.Type,
                                StartDateTime = p.StartDateTime,
                                EndDateTime = p.EndDateTime,
                                AdsTimePlaying = p.AdsTimePlaying,
                                Schedules = p.Schedules.Where(s => s.Date == null || s.Date <= planEnd).Select(s => new ScheduleDto()
                                { 
                                    StartTime = s.StartTime,
                                    EndTime = s.EndTime,
                                    BreakTime = s.BreakTime,
                                    Date = s.Date,
                                    DayOfWeek = s.DayOfWeek
                                })        
                            } : null
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

                             Plans = apvGroup.Where(apv => apv.Plan != null).Select(apv => apv.Plan),
                             DaysOff = apvGroup.Where(apv => apv.DaysOff != null).Select(apv => apv.DaysOff.Value)
                         };

            return result.ToList();
        }
 
    }
}

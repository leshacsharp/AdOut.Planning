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

        public Task<List<AdPointValidation>> GetAdPointsValidationAsync(string planId, DateTime planStart, DateTime planEnd)
        {
            var query = Context.AdPoints.Where(ap => ap.PlanAdPoints.Any(pap => pap.PlanId == planId &&
                                                                                pap.Plan.StartDateTime >= planStart &&
                                                                                pap.Plan.EndDateTime <= planEnd))
                           .Select(ap => new AdPointValidation()
                           {
                               Location = ap.Location,
                               StartWorkingTime = ap.StartWorkingTime,
                               EndWorkingTime = ap.EndWorkingTime,
                               DaysOff = ap.DaysOff.Select(doff => doff.DayOfWeek),
                               Schedules = ap.PlanAdPoints.SelectMany(pap => pap.Plan.Schedules)
                                                   .Where(s => s.Date == null || s.Date <= planEnd)
                                                   .Select(s => new ScheduleDto() 
                                                   {
                                                       StartTime = s.StartTime,
                                                       EndTime = s.EndTime,
                                                       BreakTime = s.BreakTime,
                                                       PlayTime = s.PlayTime,
                                                       Date = s.Date,
                                                       DayOfWeek = s.DayOfWeek
                                                   })
                           });

            return query.ToListAsync();
        }
    }
}

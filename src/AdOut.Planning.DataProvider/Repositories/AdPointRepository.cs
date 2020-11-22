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
            //todo: check a generated sql

            var query = Context.AdPoints.Where(ap => ap.PlanAdPoints.Any(pap => pap.PlanId == planId))                                                  
                           .Select(ap => new AdPointValidation()
                           {
                               Location = ap.Location,
                               StartWorkingTime = ap.StartWorkingTime,
                               EndWorkingTime = ap.EndWorkingTime,
                               DaysOff = ap.DaysOff.Select(doff => doff.DayOfWeek),
                               Plans = ap.PlanAdPoints.Select(pap => new PlanValidation() 
                               {
                                    StartDateTime = pap.Plan.StartDateTime,
                                    EndDateTime = pap.Plan.EndDateTime,
                                    //todo: mb need to add conditions for schedules
                                    Schedules = pap.Plan.Schedules.Select(s => new ScheduleDto()
                                    {
                                        StartTime = s.StartTime,
                                        EndTime = s.EndTime,
                                        BreakTime = s.BreakTime,
                                        PlayTime = s.PlayTime,
                                        Date = s.Date,
                                        DayOfWeek = s.DayOfWeek
                                    })
                               })                                                 
                           });

            return query.ToListAsync();
        }
    }
}

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

        public Task<List<AdPointValidation>> GetAdPointValidationsAsync(int[] adPointIds)
        {
            var query = from ap in Context.AdPoints.Where(ap => adPointIds.Contains(ap.Id))

                        select new AdPointValidation()
                        {
                            Location = ap.Location,
                            StartWorkingTime = ap.StartWorkingTime,
                            EndWorkingTime = ap.EndWorkingTime,

                            Plans = from pap in ap.PlanAdPoints

                                    join p in Context.Plans on pap.PlanId equals p.Id
                                    
                                    select new PlanDto
                                    {
                                        Type = p.Type,
                                        StartDateTime = p.StartDateTime,
                                        EndDateTime = p.EndDateTime,

                                        Schedules = p.Schedules.Select(s => new ScheduleDto() 
                                        {
                                            StartTime = s.StartTime,
                                            EndTime = s.EndTime,
                                            BreakTime = s.BreakTime,
                                            DayOfWeek = s.DayOfWeek,
                                            Date = s.Date
                                        })
                                    },

                            DaysOff = from apw in ap.AdPointDaysOff

                                       join da in Context.DaysOff on apw.DayOffId equals da.Id

                                       select .DayOfWeek
                        };

            return query.ToListAsync();
        }
    }
}

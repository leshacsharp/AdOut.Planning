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

        public Task<List<AdPointValidation>> GetAdPointValidationsAsync(int[] adPointIds, DateTime planStart, DateTime planEnd)
        {
            var query = from ap in Context.AdPoints
                        where adPointIds.Contains(ap.Id)
                        select new AdPointValidation()
                        {
                            Location = ap.Location,
                            StartWorkingTime = ap.StartWorkingTime,
                            EndWorkingTime = ap.EndWorkingTime,

                            Plans = from pap in ap.PlanAdPoints
                                    join p in Context.Plans on pap.PlanId equals p.Id
                                    where p.StartDateTime >= planStart && p.StartDateTime <= planEnd
                                    select new PlanValidation
                                    {
                                        Type = p.Type,
                                        StartDateTime = p.StartDateTime,
                                        EndDateTime = p.EndDateTime,

                                        Schedules = from s in p.Schedules
                                                    where s.Date == null || s.Date <= planEnd
                                                    select new ScheduleValidation()
                                                    {
                                                        StartTime = s.StartTime,
                                                        EndTime = s.EndTime,
                                                        BreakTime = s.BreakTime,
                                                        DayOfWeek = s.DayOfWeek,
                                                        Date = s.Date
                                                    },

                                        PlanAds = from pa in p.PlanAds
                                                  select new PlanAdValidation()
                                                  {
                                                      Order = pa.Order,
                                                      TimePlayingSec = pa.TimePlayingSec
                                                  }
                                    },

                            DaysOff = from apw in ap.AdPointDaysOff
                                      join da in Context.DaysOff on apw.DayOffId equals da.Id
                                      select da.DayOfWeek
                        };

            return query.ToListAsync();
        }
    }
}

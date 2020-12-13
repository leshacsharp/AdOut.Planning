using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class TariffRepository : BaseRepository<Tariff>, ITariffRepository
    {
        public TariffRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<List<TariffDto>> GetPlanTariffsAsync(string planId)
        {
            var query = from pap in Context.PlanAdPoints.Where(pap => pap.PlanId == planId)
                        join ap in Context.AdPoints on pap.AdPointId equals ap.Id
                        join t in Context.Tariffs on ap.Id equals t.AdPointId
                        select new TariffDto()
                        {
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            PriceForMinute = t.PriceForMinute
                        };

            return query.ToListAsync();
        }
    }
}

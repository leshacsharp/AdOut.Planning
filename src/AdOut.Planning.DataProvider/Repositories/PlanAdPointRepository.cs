using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanAdPointRepository : BaseRepository<PlanAdPoint>, IPlanAdPointRepository
    {
        public PlanAdPointRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<List<int>> GetAdPointsIds(int planId)
        {
            var query = from pap in Context.PlanAdPoints
                        where pap.PlanId == planId
                        select pap.AdPointId;

            return query.ToListAsync();
        }

        public Task<PlanAdPoint> GetByIdAsync(int planId, int adPointId)
        {
            return Context.PlanAdPoints.SingleOrDefaultAsync(pap => pap.PlanId == planId && pap.AdPointId == adPointId);
        }
    }
}

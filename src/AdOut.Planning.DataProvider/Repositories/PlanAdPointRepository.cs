using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
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

        public Task<PlanAdPoint> GetByIdAsync(int planId, int adPointId)
        {
            var query = from pap in Context.PlanAdPoints
                        where pap.PlanId == planId && pap.AdPointId == adPointId
                        select pap;

            return query.SingleOrDefaultAsync();
        }
    }
}

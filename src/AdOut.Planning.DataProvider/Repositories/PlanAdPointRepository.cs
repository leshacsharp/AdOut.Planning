using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanAdPointRepository : BaseRepository<PlanAdPoint>, IPlanAdPointRepository
    {
        public PlanAdPointRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<PlanAdPoint> GetByIdAsync(string planId, string adPointId)
        {
            return Context.PlanAdPoints.SingleOrDefaultAsync(pap => pap.PlanId == planId && pap.AdPointId == adPointId);
        }
    }
}

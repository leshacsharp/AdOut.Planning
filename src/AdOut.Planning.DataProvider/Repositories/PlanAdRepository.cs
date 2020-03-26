using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanAdRepository : BaseRepository<PlanAd>, IPlanAdRepository
    {
        public PlanAdRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<PlanAd> GetByIdAsync(int planId, int adId)
        {
            var query = from pa in Context.PlanAds
                        where pa.PlanId == planId && pa.AdId == adId
                        select pa;

            return query.SingleOrDefaultAsync();
        }
    }
}

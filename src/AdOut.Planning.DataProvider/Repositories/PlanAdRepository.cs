using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanAdRepository : BaseRepository<PlanAd>, IPlanAdRepository
    {
        public PlanAdRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<PlanAd> GetByIdAsync(string planId, string adId)
        {
            return Context.PlanAds.SingleOrDefaultAsync(pa => pa.PlanId == planId && pa.AdId == adId);
        }
    }
}

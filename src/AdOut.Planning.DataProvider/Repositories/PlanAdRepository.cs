using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanAdRepository : BaseRepository<PlanAd>, IPlanAdRepository
    {
        public PlanAdRepository(IDatabaseContext context) 
            : base(context)
        {
        }
    }
}

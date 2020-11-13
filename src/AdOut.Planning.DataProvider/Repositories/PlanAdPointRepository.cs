using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanAdPointRepository : BaseRepository<PlanAdPoint>, IPlanAdPointRepository
    {
        public PlanAdPointRepository(IDatabaseContext context) 
            : base(context)
        {
        }
    }
}

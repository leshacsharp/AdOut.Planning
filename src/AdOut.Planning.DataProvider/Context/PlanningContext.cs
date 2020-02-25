using AdOut.Planning.Model.Interfaces.Context;
using Microsoft.EntityFrameworkCore;

namespace AdOut.Planning.DataProvider.Context
{
    public class PlanningContext : DbContext, IDatabaseContext
    {
        public PlanningContext(DbContextOptions<PlanningContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
    }
}

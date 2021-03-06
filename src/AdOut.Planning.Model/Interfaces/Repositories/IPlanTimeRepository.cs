using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanTimeRepository : IBaseRepository<PlanTime>
    {
        Task<List<PlanTime>> GetPlanTimes(string[] adPointIds, DateTime planStart, DateTime planEnd);
    }
}

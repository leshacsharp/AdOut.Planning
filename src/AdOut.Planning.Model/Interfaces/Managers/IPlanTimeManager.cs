using AdOut.Planning.Model.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanTimeManager
    {
        Task<List<PlanTime>> GetTodaysPlanTimesAsync(string adPointId);
    }
}

using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<Plan> GetByIdAsync(int planId);
        Task<List<int>> GetAdPointsIds(int plaId);
        Task<List<AdPointPlanDto>> GetByAdPoint(int adPointId);
    }
}

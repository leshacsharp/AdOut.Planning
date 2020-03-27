﻿using AdOut.Planning.Model.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanAdPointRepository : IBaseRepository<PlanAdPoint>
    {
        Task<List<int>> GetAdPointsIds(int planId);
        Task<PlanAdPoint> GetByIdAsync(int planId, int adPointId);
    }
}

using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<List<AdPointPlanDto>> GetByAdPoint(string adPointId, DateTime dateFrom, DateTime dateTo);
        Task<Plan> GetByIdAsync(string planId);
        Task<PlanDto> GetDtoByIdAsync(string planId);
    }
}

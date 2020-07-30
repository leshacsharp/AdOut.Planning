using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        //todo: change name of 'AdPointPlanDto' class
        Task<List<AdPointPlanDto>> GetByAdPoint(int adPointId, DateTime dateFrom, DateTime dateTo);
        Task<Plan> GetByIdAsync(int planId);
        Task<PlanDto> GetDtoByIdAsync(int planId);
    }
}

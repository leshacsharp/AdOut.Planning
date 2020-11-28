using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<TempPlanValidation> GetTempPlanValidationAsync(string planId);
        Task<List<PlanValidation>> GetPlanValidationsAsync(string planId, DateTime planStart, DateTime planEnd);
        Task<List<AdPointPlanDto>> GetByAdPointAsync(string adPointId, DateTime dateFrom, DateTime dateTo);
        Task<PlanDto> GetDtoByIdAsync(string planId);
    }
}

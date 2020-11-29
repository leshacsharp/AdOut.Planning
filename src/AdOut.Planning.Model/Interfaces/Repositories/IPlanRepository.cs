using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        //todo: check all these entities on simularity
        Task<PlanExtensionValidation> GetPlanExtensionValidation(string planId);
        Task<SchedulePlanValidation> GetPlanScheduleValidationAsync(string planId);
        Task<List<PlanValidation>> GetPlanValidationsAsync(string planId, DateTime planStart, DateTime planEnd);
        Task<List<PlanTimeLine>> GetPlanTimeLinesAsync(string adPointId, DateTime dateFrom, DateTime dateTo);
        Task<PlanDto> GetDtoByIdAsync(string planId);
    }
}

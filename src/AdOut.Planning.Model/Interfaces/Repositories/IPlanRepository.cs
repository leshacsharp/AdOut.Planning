using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<PlanExtensionValidation> GetPlanExtensionValidation(string planId);
        Task<ScheduleValidation> GetScheduleValidationAsync(string planId);
        Task<List<PlanTimeLine>> GetPlanTimeLinesAsync(string planId, DateTime planStart, DateTime planEnd);
        Task<PlanDto> GetDtoByIdAsync(string planId);
    }
}

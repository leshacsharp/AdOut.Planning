using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<List<PlanTimeLine>> GetPlanTimeLinesAsync(string[] adPointIds, DateTime planStart, DateTime planEnd);
        Task<PlanExtensionValidation> GetPlanExtensionValidationAsync(string planId);
        //todo: rename to GetPlanValidationAsync
        Task<ScheduleValidation> GetScheduleValidationAsync(string planId);    
        Task<PlanTimeDto> GetPlanTimeAsync(string planId);
        Task<PlanPriceDto> GetPlanPriceAsync(string planId);
        Task<PlanDto> GetDtoByIdAsync(string planId);
    }
}

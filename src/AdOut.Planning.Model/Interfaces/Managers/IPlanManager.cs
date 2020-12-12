using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanManager : IBaseManager<Plan>
    {
        double CalculatePlanPrice(List<ScheduleTime> schedulesTimes, List<TariffDto> tariffs);
        Task<List<PlanPeriod>> GetPlansTimeLines(string adPointId, DateTime dateFrom, DateTime dateTo);

        Task<ValidationResult<string>> ValidatePlanExtensionAsync(string planId, DateTime newEndDate);

        Task ExtendPlanAsync(string planId, DateTime newEndDate);

        void Create(CreatePlanModel createModel);

        Task UpdateAsync(UpdatePlanModel updateModel);

        Task DeleteAsync(string planId);

        Task<PlanDto> GetDtoByIdAsync(string planId);

        Task<Plan> GetByIdAsync(string planId);
    }
}

using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanManager : IBaseManager<Plan>
    {
        Task<ValidationResult<string>> ValidatePlanExtensionAsync(int planId, DateTime newEndDate);

        void Create(CreatePlanModel createModel, string userId);

        Task UpdateAsync(UpdatePlanModel updateModel);

        Task DeleteAsync(int planId);

        Task<PlanDto> GetDtoByIdAsync(int planId);

        Task<Plan> GetByIdAsync(int planId);

        Task ExtendPlanAsync(int planId, DateTime newEndDate);
    }
}

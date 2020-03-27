﻿using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using System;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanManager : IBaseManager<Plan>
    {
        void Create(CreatePlanModel createModel, string userId);

        Task UpdateAsync(UpdatePlanModel updateModel);

        Task<ValidationResult<string>> ValidatePlanExtension(int planId, DateTime newEndDate);

        Task ExtendPlan(int planId, DateTime newEndDate);

        Task AddAdAsync(int planId, int adId, int order);

        Task DeleteAdAsync(int planId, int adId);

        Task UpdateAdAsync(int planId, int adId, int order);
    }
}

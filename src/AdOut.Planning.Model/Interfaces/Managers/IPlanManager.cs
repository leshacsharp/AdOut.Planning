﻿using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanManager
    {
        Task<ValidationResult<string>> ValidatePlanExtensionAsync(string planId, DateTime newEndDate);
        Task ExtendPlanAsync(string planId, DateTime newEndDate);
        void Create(CreatePlanModel createModel);
        Task UpdateAsync(UpdatePlanModel updateModel);
        Task DeleteAsync(string planId);
        Task<PlanDto> GetDtoByIdAsync(string planId);
    }
}

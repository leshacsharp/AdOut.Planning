using AdOut.Extensions.Context;
using AdOut.Extensions.Exceptions;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPlanManager _planManager;
        private readonly ICommitProvider _commitProvider;
        private readonly IAuthorizationService _authorizationService;

        public PlanController(
            IPlanManager planManager,
            ICommitProvider commitProvider,
            IAuthorizationService authorizationService)
        {
            _planManager = planManager;
            _commitProvider = commitProvider;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("plans-timelines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlansTimeLines(string adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var plansTimeLines = await _planManager.GetPlansTimeLines(adPointId, dateFrom, dateTo);
            return Ok(plansTimeLines);
        }

        [HttpPut]
        [Route("extend-plan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExtendPlan(string planId, DateTime newEndDate)
        {
            await CheckUserPermissionsForResourceAsync(planId);

            var validationResult = await _planManager.ValidatePlanExtensionAsync(planId, newEndDate);
            if (!validationResult.IsSuccessed)
            {
                return BadRequest(validationResult.Errors);
            }

            await _planManager.ExtendPlanAsync(planId, newEndDate);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("plan/{id}")]
        [ProducesResponseType(typeof(PlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlan(string id)
        {
            var plan = await _planManager.GetDtoByIdAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            var authResult = await _authorizationService.AuthorizeAsync(User, plan, AuthPolicies.ResourcePolicy);
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            return Ok(plan);
        }

        [HttpPost]
        [Route("plan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreatePlan(CreatePlanModel createModel)
        {
            _planManager.Create(createModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("plan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePlan(string id)
        {
            await CheckUserPermissionsForResourceAsync(id);

            await _planManager.DeleteAsync(id);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("plan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePlan(UpdatePlanModel updateModel)
        {
            await CheckUserPermissionsForResourceAsync(updateModel.PlanId);

            await _planManager.UpdateAsync(updateModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        private async Task CheckUserPermissionsForResourceAsync(string planId)
        {
            var plan = await _planManager.GetByIdAsync(planId);
            var authResult = await _authorizationService.AuthorizeAsync(User, plan, AuthPolicies.ResourcePolicy);

            if (!authResult.Succeeded)
            {
                throw new ForbiddenException();
            }
        }
    }
}
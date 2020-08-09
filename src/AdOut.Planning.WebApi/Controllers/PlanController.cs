using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        [Route("{id}")]
        [ProducesResponseType(typeof(PlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlan(string id)
        {
            var plan = await _planManager.GetByIdAsync(id);
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
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreatePlan(CreatePlanModel createModel)
        {
            var userId = User.GetUserId();
            //todo: delete
            userId = "test-user";
            _planManager.Create(createModel, userId);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePlan(string id)
        {
            await CheckUserPermissionsForResourceAsync(id);

            await _planManager.DeleteAsync(id);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("update")]
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
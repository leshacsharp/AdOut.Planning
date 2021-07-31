using AdOut.Extensions.Authorization;
using AdOut.Extensions.Context;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/v1")]
    [ApiController]
    //[Authorize]
    public class PlanController : ControllerBase
    {
        private readonly IPlanManager _planManager;
        private readonly IPlanTimeManager _planTimeManager;
        private readonly ICommitProvider _commitProvider;

        public PlanController(
            IPlanManager planManager,
            IPlanTimeManager planTimeManager,
            ICommitProvider commitProvider)
        {
            _planManager = planManager;
            _planTimeManager = planTimeManager;
            _commitProvider = commitProvider;
        }

        [Authorize]
        [HttpGet]
        [Route("stream-plans")]
        //[ResourceAuthorization(typeof(Plan))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStreamPlans([ResourceId] string adPointId, DateTime date)
        {
            var plansTimeLines = await _planTimeManager.GetStreamPlansTimeAsync(adPointId, date);
            return Ok(plansTimeLines);
        }

        [HttpPut]
        [Route("extend-plan")]
        //[ResourceAuthorization(typeof(Plan))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExtendPlan([ResourceId] string planId, DateTime newEndDate)
        {
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
        //[ResourceAuthorization(typeof(Plan))]
        [ProducesResponseType(typeof(PlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlan(string id)
        {     
            var plan = await _planManager.GetDtoByIdAsync(id);
            if (plan == null)
            {
                return NotFound();
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
        //[ResourceAuthorization(typeof(Plan))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeletePlan(string id)
        {
            await _planManager.DeleteAsync(id);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }
     
        [HttpPut]
        [Route("plan")]
        //[ResourceAuthorization(typeof(Plan))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdatePlan(UpdatePlanModel updateModel)
        {
            await _planManager.UpdateAsync(updateModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }
    }
}
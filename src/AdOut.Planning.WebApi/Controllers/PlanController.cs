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
    [Authorize]
    public class PlanController : ControllerBase
    {
        private readonly IPlanManager _planManager;
        private readonly ICommitProvider _commitProvider;

        public PlanController(
            IPlanManager planManager,
            ICommitProvider commitProvider)
        {
            _planManager = planManager;
            _commitProvider = commitProvider;
        }

        [HttpGet]
        [Route("plans-timelines")]
        //[ResourceAuthorization(typeof(Plan), "adPointId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlansTimeLines(string adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var plansTimeLines = await _planManager.GetPlansTimeLines(adPointId, dateFrom, dateTo);
            return Ok(plansTimeLines);
        }

        [HttpPut]
        [Route("extend-plan")]
        //[ResourceAuthorization(typeof(Plan), "planId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExtendPlan(string planId, DateTime newEndDate)
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
        public async Task<IActionResult> UpdatePlan(UpdatePlanModel updateModel)
        {
            await _planManager.UpdateAsync(updateModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }
    }
}
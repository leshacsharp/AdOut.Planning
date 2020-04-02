﻿using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.WebApi.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPut]
        [Route("extend-plan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExtendPlan(int planId, DateTime newEndDate)
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
        [Route("{id}")]
        [ProducesResponseType(typeof(PlanDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlan(int id)
        {
            var plan = await _planManager.GetByIdAsync(id);
            return Ok(plan);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreatePlan(CreatePlanModel createModel)
        {
            var userId = User.GetUserId();

            _planManager.Create(createModel, userId);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreatePlan(int id)
        {
            await _planManager.DeleteAsync(id);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePlan(UpdatePlanModel updateModel)
        {
            await _planManager.UpdateAsync(updateModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpPost]
        [Route("add-ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAdToPlan(int planId, int adId, int order)
        {
            await _planManager.AddAdAsync(planId, adId, order);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("delete-ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAdFromPlan(int planId, int adId)
        {
            await _planManager.DeleteAdAsync(planId, adId);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("update-ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAdInPlan(int planId, int adId, int order)
        {
            await _planManager.UpdateAdAsync(planId, adId, order);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }
    }
}
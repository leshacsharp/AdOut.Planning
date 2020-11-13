using AdOut.Planning.Model.Api;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleManager _scheduleManager;
        private readonly IPlanManager _planManager;
        private readonly ICommitProvider _commitProvider;
        private readonly IAuthorizationService _authorizationService;

        public ScheduleController(
            IScheduleManager scheduleManager,
            IPlanManager planManager,
            ICommitProvider commitProvider,
            IAuthorizationService authorizationService)
        {
            _scheduleManager = scheduleManager;
            _planManager = planManager;
            _commitProvider = commitProvider;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ScheduleModel createModel)
        {
            var validationResult = await _scheduleManager.ValidateScheduleAsync(createModel);
            if (!validationResult.IsSuccessed)
            {
                return BadRequest(validationResult.Errors);
            }

            await _scheduleManager.CreateAsync(createModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UpdateScheduleModel updateModel)
        {
            await CheckUserPermissionsForResourceAsync(updateModel.PlanId);

            //wtf?
            var scheduleModel = new ScheduleModel()
            {
                PlanId = updateModel.PlanId,
                StartTime = updateModel.StartTime,
                EndTime = updateModel.EndTime,
                BreakTime = updateModel.BreakTime,
                Date = updateModel.Date,
                DayOfWeek = updateModel.DayOfWeek
            };

            var validationResult = await _scheduleManager.ValidateScheduleAsync(scheduleModel);
            if (!validationResult.IsSuccessed)
            {
                return BadRequest(validationResult.Errors);
            }

            await _scheduleManager.UpdateAsync(updateModel);
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
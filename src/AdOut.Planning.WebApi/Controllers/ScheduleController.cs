using AdOut.Extensions.Context;
using AdOut.Extensions.Exceptions;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleManager _scheduleManager;
        private readonly IPlanManager _planManager;
        private readonly ICommitProvider _commitProvider;

        public ScheduleController(
            IScheduleManager scheduleManager,
            IPlanManager planManager,
            ICommitProvider commitProvider)
        {
            _scheduleManager = scheduleManager;
            _planManager = planManager;
            _commitProvider = commitProvider;
        }

        [HttpPost]
        [Route("schedule")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSchedule(ScheduleModel createModel)
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
        [Route("schedule")]
        //[ResourceAuthorization(typeof(Schedule))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateSchedule(UpdateScheduleModel updateModel)
        {
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

            //todo: before validating existing schedule, need to delete it from the list
            var validationResult = await _scheduleManager.ValidateScheduleAsync(scheduleModel);
            if (!validationResult.IsSuccessed)
            {
                return BadRequest(validationResult.Errors);
            }

            await _scheduleManager.UpdateAsync(updateModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }
    }
}
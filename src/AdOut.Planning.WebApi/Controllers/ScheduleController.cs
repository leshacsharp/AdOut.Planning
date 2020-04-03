using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleManager _scheduleManager;
        private readonly ICommitProvider _commitProvider;

        public ScheduleController(
            IScheduleManager scheduleManager,
            ICommitProvider commitProvider)
        {
            _scheduleManager = scheduleManager;
            _commitProvider = commitProvider;
        }

        [HttpGet]
        [Route("plans-timelines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlansTimeLines(int adPointId, DateTime dateFrom, DateTime dateTo)
        {
            var plansTimeLines = await _scheduleManager.GetPlansTimeLines(adPointId, dateFrom, dateTo);
            return Ok(plansTimeLines);
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
    }
}
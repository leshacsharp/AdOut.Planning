using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Dto;
using AdOut.Extensions.Context;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Extensions.Communication.Interfaces;
using AdOut.Planning.Model.Events;

namespace AdOut.Planning.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IPlanManager _planManager;
        private readonly ITariffRepository _tariffRepository;
        private readonly ICommitProvider _commitProvider;
        private readonly IDatabaseContext _context;
        private readonly IMessageBroker _broker;
        private readonly IPlanTimeManager _planTimeManager;

        public HomeController(
            IScheduleRepository scheduleRepository, 
            IPlanRepository planRepository,
            IPlanManager planManager,
            ITariffRepository tariffRepository,
            ICommitProvider commitProvider,
            IDatabaseContext context,
            IMessageBroker broker,
            IPlanTimeManager planTimeManager)
        {
            _scheduleRepository = scheduleRepository;
            _planRepository = planRepository;
            _planManager = planManager;
            _tariffRepository = tariffRepository;
            _commitProvider = commitProvider;
            _context = context;
            _broker = broker;
            _planTimeManager = planTimeManager;
        }

        [HttpPost]
        [Route("accept-plan")]
        public IActionResult AcceptPlan(string planId)
        {
            var acceptEvent = new PlanAcceptedEvent() { Id = planId, Creator = "test" };
            _broker.Publish(acceptEvent);
            return Ok();
        }

        [HttpGet]
        [Route("plan-times")]
        public async Task<IActionResult> GetPlanTimes(string adPointId)
        {
            var result = await _planTimeManager.GetTodaysStreamPlanTimesAsync(adPointId);
            return Ok(result);
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            var plan = new Plan()
            {
                Id = "1",
                Title = "test1",
                Status = Model.Enum.PlanStatus.OnModeration,
                Creator = "test"
            };

            _context.ChangeTracker.Context.Entry(plan).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

          //  _context.Attach(plan);
            //_planRepository.Update(plan);
            await _commitProvider.SaveChangesAsync(false);

            //var start = new DateTime(2021, 1, 10);
            //var end = new DateTime(2021, 1, 20);
            //var c = await _planRepository.GetPlanTimeLinesAsync(new[] { "1" }, start, end);

            //var b = await _planRepository.GetPlanPriceAsync("1");
            //var a = await _tariffRepository.GetPlanTariffsAsync("1");

            //var scheduleTimes = new List<ScheduleTime>()
            //{
            //    new ScheduleTime()
            //    {
            //        PlanStartDateTime = new DateTime(2020, 1, 1),
            //        PlanEndDateTime = new DateTime(2020, 1, 2),
            //        ScheduleType = Model.Enum.ScheduleType.Daily,
            //        ScheduleStartTime = new TimeSpan(0, 0, 0),
            //        ScheduleEndTime = new TimeSpan(23, 59, 0),
            //        AdPlayTime = new TimeSpan(4, 0, 0 ),
            //        AdBreakTime = new TimeSpan(0, 40, 0),
            //        AdPointsDaysOff = new List<DayOfWeek>()
            //    }
            //};

            //var tariffs = new List<TariffDto>()
            //{
            //    new TariffDto()
            //    {
            //        StartTime = new TimeSpan(0, 0, 0),
            //        EndTime = new TimeSpan(12, 0, 0),
            //        PriceForMinute = 1
            //    },
            //    new TariffDto()
            //    {
            //        StartTime = new TimeSpan(12, 0, 0),
            //        EndTime = new TimeSpan(23, 59, 0),
            //        PriceForMinute = 2
            //    },
            //};

            //var price = _planManager.CalculatePlanPrice(scheduleTimes, tariffs);
            return Ok();
        }
    }
}

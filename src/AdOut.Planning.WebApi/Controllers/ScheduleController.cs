using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //public Task<IActionResult> Validate()
        //{

        //}
    }
}
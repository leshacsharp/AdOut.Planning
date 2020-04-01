using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Api;

namespace AdOut.Planning.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IPlanManager _planManager;

        public HomeController(IPlanManager p)
        {

            _planManager = p;
        }

        //[HttpPost]
        //[Route("plan")]
        //public async Task<IActionResult> Plan(CreatePlanModel m)
        //{
        //    _planManager.Create(m, "testUserId");
        //    return Ok();
        //}

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello world and AdOut.Planning, good luck (-:");
        }
    }
}

using AdOut.Extensions.Context;
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
    public class PlanAdController : ControllerBase
    {    
        private readonly IPlanAdManager _planAdManager;
        private readonly IPlanManager _planManager;
        private readonly ICommitProvider _commitProvider;

        public PlanAdController(
            IPlanAdManager planAdManager,
            IPlanManager planManager,
            ICommitProvider commitProvider)
        {
            _planAdManager = planAdManager;
            _planManager = planManager;
            _commitProvider = commitProvider;
        }

        [HttpPost]
        [Route("plan-ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAdToPlan(string planId, string adId, int order)
        {
            await _planAdManager.AddAdToPlanAsync(planId, adId, order);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("plan-ad")]
        //[ResourceAuthorization(typeof(Plan), "planId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAdFromPlan(string planId, string adId)
        {
            await _planAdManager.DeleteAdFromPlanAsync(planId, adId);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("plan-ad")]
        //[ResourceAuthorization(typeof(Plan), "planId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAdInPlan(string planId, string adId, int order)
        {
            await _planAdManager.UpdateAdInPlanAsync(planId, adId, order);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        } 
    }
}
using System.Threading.Tasks;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanAdController : ControllerBase
    {    
        private readonly IPlanAdManager _planAdManager;
        private readonly IPlanManager _planManager;
        private readonly IAdManager _adManager;
        private readonly ICommitProvider _commitProvider;
        private readonly IAuthorizationService _authorizationService;

        public PlanAdController(
            IPlanAdManager planAdManager,
            IPlanManager planManager,
            IAdManager adManager,
            ICommitProvider commitProvider,
            IAuthorizationService authorizationService)
        {
            _planAdManager = planAdManager;
            _planManager = planManager;
            _adManager = adManager;
            _commitProvider = commitProvider;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("add-ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAdToPlan(int planId, int adId, int order)
        {
            await CheckUserPermissionsForResourceAsync(planId, adId);

            await _planAdManager.AddAdToPlanAsync(planId, adId, order);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("delete-ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAdFromPlan(int planId, int adId)
        {
            await CheckUserPermissionsForResourceAsync(planId, adId);

            await _planAdManager.DeleteAdFromPlanAsync(planId, adId);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("update-ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAdInPlan(int planId, int adId, int order)
        {
            await CheckUserPermissionsForResourceAsync(planId, adId);

            await _planAdManager.UpdateAdInPlanAsync(planId, adId, order);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        private async Task CheckUserPermissionsForResourceAsync(int planId, int adId)
        {
            var plan = await _planManager.GetByIdAsync(planId);
            var planAuth = await _authorizationService.AuthorizeAsync(User, plan, AuthPolicies.ResourcePolicy);

            if (!planAuth.Succeeded)
            {
                throw new ForbiddenException();
            }

            var ad = await _adManager.GetByIdAsync(planId);
            var adAuth = await _authorizationService.AuthorizeAsync(User, ad, AuthPolicies.ResourcePolicy);

            if (!adAuth.Succeeded)
            {
                throw new ForbiddenException();
            }
        }
    }
}
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdManager _adManager;
        private readonly ICommitProvider _commitProvider;
        private readonly IAuthorizationService _authorizationService;

        public AdController(
            IAdManager adManager,
            ICommitProvider commitProvider,
            IAuthorizationService authorizationService)
        {
            _adManager = adManager;
            _commitProvider = commitProvider;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<ContentError>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAd(CreateAdModel createModel)
        {
            var validationResult = await _adManager.ValidateAsync(createModel.Content);
            if (!validationResult.IsSuccessed)
            {
                return BadRequest(validationResult.Errors);
            }

            await _adManager.CreateAsync(createModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpGet]
        [Route("ads")]
        [ProducesResponseType(typeof(List<AdListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAds([FromQuery]AdsFilterModel filter)
        {
            var ads = await _adManager.GetAdsAsync(filter);
            return Ok(ads);
        }

        [HttpGet]
        [Route("ad/{id}")]
        [ProducesResponseType(typeof(AdDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAd(string id)
        {
            var ad = await _adManager.GetDtoByIdAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            var authResult = await _authorizationService.AuthorizeAsync(User, ad, AuthPolicies.ResourcePolicy);
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            return Ok(ad);
        }

        [HttpPut]
        [Route("ad")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAd(UpdateAdModel updateModel)
        {
            await CheckUserPermissionsForResourceAsync(updateModel.AdId);

            await _adManager.UpdateAsync(updateModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("ad/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAd(string id)
        {
            await CheckUserPermissionsForResourceAsync(id);

            await _adManager.DeleteAsync(id);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        private async Task CheckUserPermissionsForResourceAsync(string adId)
        {  
            var ad = await _adManager.GetByIdAsync(adId);
            var authResult = await _authorizationService.AuthorizeAsync(User, ad, AuthPolicies.ResourcePolicy);

            if (!authResult.Succeeded)
            {
                throw new ForbiddenException();
            }
        }
    }
}
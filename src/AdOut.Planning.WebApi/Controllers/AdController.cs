using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdManager _adManager;
        private readonly ICommitProvider _commitProvider;

        public AdController(
            IAdManager adManager,
            ICommitProvider commitProvider)
        {
            _adManager = adManager;
            _commitProvider = commitProvider;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<ContentError>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateAdModel createModel)
        {
            var validationResult = await _adManager.ValidateAsync(createModel.Content);
            if (!validationResult.IsSuccessed)
            {
                return BadRequest(validationResult.Errors);
            }

            //todo: get userId from claims
            await _adManager.CreateAsync(createModel, null);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("ads")]
        [ProducesResponseType(typeof(List<AdListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAds(AdsFilterModel filter)
        {
            //todo: get userId from claims
            var ads = await _adManager.GetAdsAsync(filter, null);
            return Ok(ads);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(AdDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAd(int id)
        {
            var ad = await _adManager.GetByIdAsync(id);
            return Ok(ad);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateAdModel updateModel)
        {
            await _adManager.UpdateAsync(updateModel);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            await _adManager.DeleteAsync(id);
            await _commitProvider.SaveChangesAsync();

            return NoContent();
        }
    }
}
using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IAdManager : IBaseManager<Ad>
    {
        Task<ValidationResult<ContentError>> ValidateAsync(IFormFile content);
        Task<List<AdListDto>> GetAdsAsync(AdsFilterModel filterModel, string userId);
        Task CreateAsync(CreateAdModel createModel, string userId);
        Task UpdateAsync(UpdateAdModel updateModel);
        Task DeleteAsync(int adId);
        Task<AdDto> GetDtoByIdAsync(int adId);
        Task<Ad> GetByIdAsync(int adId);
    }
}

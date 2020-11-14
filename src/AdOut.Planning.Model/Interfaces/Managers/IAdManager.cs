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
        Task<ValidationResult<string>> ValidateAsync(IFormFile content);
        Task<List<AdListDto>> GetAdsAsync(AdsFilterModel filterModel);
        Task CreateAsync(CreateAdModel createModel);
        Task UpdateAsync(UpdateAdModel updateModel);
        Task DeleteAsync(string adId);
        Task<AdDto> GetDtoByIdAsync(string adId);
        Task<Ad> GetByIdAsync(string adId);
    }
}

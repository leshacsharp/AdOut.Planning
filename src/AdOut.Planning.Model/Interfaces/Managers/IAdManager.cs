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
        Task<ValidationResult<ContentError>> ValidateAsync(IFormFile file);
        Task<List<AdDto>> GetAds(AdsFilterModel filterModel);
        Task CreateAsync(CreateAdModel createModel);
        Task UpdateAsync(UpdateAdModel updateModel);
        Task DeleteAsync(int adId);
    }
}

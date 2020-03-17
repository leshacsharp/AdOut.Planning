using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IAdManager : IBaseManager<Ad>
    {
        Task<ValidationResult<ContentError>> ValidateAsync(IFormFile file);
        Task CreateAdAsync(CreateAdModel createAdModel);
    }
}

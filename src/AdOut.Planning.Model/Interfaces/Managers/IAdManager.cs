using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IAdManager : IBaseManager<Ad>
    {
        Task CreateAdAsync(CreateAdModel createAdModel);
    }
}

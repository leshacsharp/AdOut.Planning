using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanManager : IBaseManager<Plan>
    {
        void Create(CreatePlanModel createModel, string userId);

        Task AddAdAsync(int planId, int adId, int order);

        Task DeleteAdAsync(int planId, int adId);
    }
}

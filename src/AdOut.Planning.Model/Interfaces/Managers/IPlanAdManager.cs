using AdOut.Planning.Model.Database;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanAdManager : IBaseManager<PlanAd>
    {
        Task AddAdToPlanAsync(int planId, int adId, int order);

        Task DeleteAdFromPlanAsync(int planId, int adId);

        Task UpdateAdInPlanAsync(int planId, int adId, int order);
    }
}

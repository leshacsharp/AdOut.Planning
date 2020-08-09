using AdOut.Planning.Model.Database;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanAdManager : IBaseManager<PlanAd>
    {
        Task AddAdToPlanAsync(string planId, string adId, int order);

        Task DeleteAdFromPlanAsync(string planId, string adId);

        Task UpdateAdInPlanAsync(string planId, string adId, int order);
    }
}

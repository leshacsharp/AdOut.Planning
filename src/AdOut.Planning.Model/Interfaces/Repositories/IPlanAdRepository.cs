using AdOut.Planning.Model.Database;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanAdRepository : IBaseRepository<PlanAd>
    {
        Task<PlanAd> GetByIdAsync(string planId, string adId);
    }
}

using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Database;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanManager : IBaseManager<Plan>
    {
        void Create(CreatePlanModel createModel, string userId);
    }
}

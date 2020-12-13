using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface ITariffRepository : IBaseRepository<Tariff>
    {
        Task<List<TariffDto>> GetPlanTariffsAsync(string planId);
    }
}

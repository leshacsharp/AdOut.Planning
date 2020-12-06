using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class TariffRepository : BaseRepository<Tariff>, ITariffRepository
    {
        public TariffRepository(IDatabaseContext context) 
            : base(context)
        {
        }
    }
}

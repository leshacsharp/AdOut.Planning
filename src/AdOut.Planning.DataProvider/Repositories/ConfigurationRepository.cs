using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class ConfigurationRepository : BaseRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(IDatabaseContext context)
            : base(context)
        {

        }
    }
}

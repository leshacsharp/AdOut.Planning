using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class ConfigurationRepository : BaseRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(IDatabaseContext context)
            : base(context)
        {
        }

        public async Task<string> GetByTypeAsync(string configType)
        {
            var config = await Context.Configurations.SingleOrDefaultAsync(c => c.Type == configType);
            return config.Value;
        }
    }
}

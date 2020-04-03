using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class ConfigurationRepository : BaseRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(IDatabaseContext context)
            : base(context)
        {

        }

        public Task<string> GetByTypeAsync(string configType)
        {
            var query = from c in Table
                        where c.Type == configType
                        select c.Value;

            return query.SingleAsync();
        }
    }
}

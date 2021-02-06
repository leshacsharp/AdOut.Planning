using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Database;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IConfigurationRepository : IBaseRepository<Configuration>
    {
        Task<string> GetByTypeAsync(string configType);
    }
}

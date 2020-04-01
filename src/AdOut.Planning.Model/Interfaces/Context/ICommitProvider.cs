using System.Threading;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Context
{
    public interface ICommitProvider
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

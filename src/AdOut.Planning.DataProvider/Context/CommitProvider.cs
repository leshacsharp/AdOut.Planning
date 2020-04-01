using AdOut.Planning.Model.Interfaces.Context;
using System.Threading;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Context
{
    public class CommitProvider : ICommitProvider
    {
        private readonly IDatabaseContext _context;
        public CommitProvider(IDatabaseContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}

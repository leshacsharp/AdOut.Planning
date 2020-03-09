using AdOut.Planning.Model.Interfaces.Context;
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

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Context
{
    public interface ICommitProvider
    {
        Task SaveChangesAsync();
    }
}

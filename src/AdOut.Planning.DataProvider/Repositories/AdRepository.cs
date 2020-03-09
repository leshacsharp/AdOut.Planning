using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class AdRepository : BaseRepository<Ad>, IAdRepository
    {
        public AdRepository(IDatabaseContext context) 
            : base(context)
        {
        }
    }
}

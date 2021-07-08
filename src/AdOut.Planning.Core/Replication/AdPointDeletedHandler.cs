using AdOut.Extensions.Communication.Interfaces;
using AdOut.Extensions.Context;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Replication
{
    public class AdPointDeletedHandler : IReplicationHandler<AdPoint>
    {
        private readonly IAdPointRepository _adPointRepository;
        private readonly ICommitProvider _commitProvider;

        public AdPointDeletedHandler(
            IAdPointRepository adPointRepository,
            ICommitProvider commitProvider)
        {
            _adPointRepository = adPointRepository;
            _commitProvider = commitProvider;
        }

        public async Task HandleAsync(AdPoint entity)
        {
            var dbEntity = await _adPointRepository.GetByIdAsync(entity.Id);
            if (dbEntity != null)
            {
                _adPointRepository.Delete(dbEntity);
                await _commitProvider.SaveChangesAsync(false);
            } 
        }
    }
}

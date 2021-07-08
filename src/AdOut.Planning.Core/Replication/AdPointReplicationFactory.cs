using AdOut.Extensions.Communication;
using AdOut.Extensions.Communication.Interfaces;
using AdOut.Extensions.Context;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.Core.Replication
{
    public class AdPointReplicationFactory : IReplicationHandlerFactory<AdPoint>
    {
        private readonly IAdPointRepository _adPointRepository;
        private readonly ICommitProvider _commitProvider;

        public AdPointReplicationFactory(
            IAdPointRepository adPointRepository,
            ICommitProvider commitProvider)
        {
            _adPointRepository = adPointRepository;
            _commitProvider = commitProvider;
        }

        public IReplicationHandler<AdPoint> CreateReplicationHandler(EventAction action)
        {
            return action switch
            {
                EventAction.Created => new AdPointCreatedHandler(_adPointRepository, _commitProvider),
                EventAction.Deleted => new AdPointDeletedHandler(_adPointRepository, _commitProvider),
                _ => null
            };
        }
    }
}

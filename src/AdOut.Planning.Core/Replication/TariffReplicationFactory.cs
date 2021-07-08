using AdOut.Extensions.Communication;
using AdOut.Extensions.Communication.Interfaces;
using AdOut.Extensions.Context;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.Core.Replication
{
    public class TariffReplicationFactory : IReplicationHandlerFactory<Tariff>
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly ICommitProvider _commitProvider;

        public TariffReplicationFactory(
            ITariffRepository tariffRepository,
            ICommitProvider commitProvider)
        {
            _tariffRepository = tariffRepository;
            _commitProvider = commitProvider;
        }

        public IReplicationHandler<Tariff> CreateReplicationHandler(EventAction action)
        {
            return action switch
            {
                EventAction.Created => new TariffCreatedHandler(_tariffRepository, _commitProvider),
                _ => null
            };
        }
    }
}

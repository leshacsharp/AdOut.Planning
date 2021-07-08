using AdOut.Extensions.Communication.Interfaces;
using AdOut.Extensions.Context;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Replication
{
    public class TariffCreatedHandler : IReplicationHandler<Tariff>
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly ICommitProvider _commitProvider;

        public TariffCreatedHandler(
            ITariffRepository tariffRepository,
            ICommitProvider commitProvider)
        {
            _tariffRepository = tariffRepository;
            _commitProvider = commitProvider;
        }

        public async Task HandleAsync(Tariff entity)
        {
            var dbEntity = await _tariffRepository.GetByIdAsync(entity.Id);
            if (dbEntity == null)
            {
                _tariffRepository.Create(entity);
                await _commitProvider.SaveChangesAsync(false);
            }
        }
    }
}

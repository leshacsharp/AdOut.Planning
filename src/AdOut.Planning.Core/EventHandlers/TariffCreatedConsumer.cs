using AdOut.Extensions.Communication;
using AdOut.Extensions.Context;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Repositories;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.EventHandlers
{
    public class TariffCreatedConsumer : BaseConsumer<TariffCreatedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public TariffCreatedConsumer(
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        protected override Task HandleAsync(TariffCreatedEvent deliveredEvent)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var tariffRepository = scope.ServiceProvider.GetRequiredService<ITariffRepository>();
            var commitProvider = scope.ServiceProvider.GetRequiredService<ICommitProvider>();

            var tariff = _mapper.Map<Tariff>(deliveredEvent);
            tariffRepository.Create(tariff);

            return commitProvider.SaveChangesAsync();
        }
    }
}

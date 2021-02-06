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
    public class AdPointCreatedConsumer : BaseConsumer<AdPointCreatedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public AdPointCreatedConsumer(
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        protected override Task HandleAsync(AdPointCreatedEvent deliveredEvent)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var adPointRepository = scope.ServiceProvider.GetRequiredService<IAdPointRepository>();
            var commitProvider = scope.ServiceProvider.GetRequiredService<ICommitProvider>();

            var adPoint = _mapper.Map<AdPoint>(deliveredEvent);
            adPointRepository.Create(adPoint);

            return commitProvider.SaveChangesAsync(false);
        }
    }
}

using AdOut.Planning.Core.EventHandlers.Base;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.EventHandlers
{
    public class AdPointCreatedConsumer : BaseConsumer<AdPointCreatedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AdPointCreatedConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task HandleAsync(AdPointCreatedEvent deliveredEvent)
        {
            //todo: make services scoped and take off IServiceScopeFactory
            using var scope = _serviceScopeFactory.CreateScope();
            var adPointRepository = scope.ServiceProvider.GetRequiredService<IAdPointRepository>();
            var commitProvider = scope.ServiceProvider.GetRequiredService<ICommitProvider>();

            var mapperCfg = new MapperConfiguration(cfg => cfg.CreateMap(deliveredEvent.GetType(), typeof(AdPoint)));
            var mapper = new Mapper(mapperCfg);

            var adPoint = mapper.Map<AdPoint>(deliveredEvent);
            adPointRepository.Create(adPoint);

            return commitProvider.SaveChangesAsync();
        }
    }
}

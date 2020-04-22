using AdOut.Planning.Core.EventHandlers.Base;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using AutoMapper;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.EventHandlers
{
    public class AdPointCreatedConsumer : BaseConsumer<AdPointCreatedEvent>
    {
        private readonly IAdPointRepository _adPointRepository;
        private readonly ICommitProvider _commitProvider;

        public AdPointCreatedConsumer(
            IAdPointRepository adPointRepository,
            ICommitProvider commitProvider)
        {
            _adPointRepository = adPointRepository;
            _commitProvider = commitProvider;
        }

        protected override Task HandleAsync(AdPointCreatedEvent deliveredEvent)
        {
            var mapperCfg = new MapperConfiguration(cfg => cfg.CreateMap(deliveredEvent.GetType(), typeof(AdPoint)));
            var mapper = new Mapper(mapperCfg);

            var adPoint = mapper.Map<AdPoint>(deliveredEvent);
            _adPointRepository.Create(adPoint);

            return _commitProvider.SaveChangesAsync();
        }
    }
}

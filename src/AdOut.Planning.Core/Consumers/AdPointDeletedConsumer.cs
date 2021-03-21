using AdOut.Extensions.Communication;
using AdOut.Extensions.Context;
using AdOut.Extensions.Exceptions;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Consumers
{
    public class AdPointDeletedConsumer : BaseConsumer<AdPointDeletedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AdPointDeletedConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleAsync(AdPointDeletedEvent deliveredEvent)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var adPointRepository = scope.ServiceProvider.GetRequiredService<IAdPointRepository>();
            var commitProvider = scope.ServiceProvider.GetRequiredService<ICommitProvider>();

            var adPoint = await adPointRepository.GetByIdAsync(deliveredEvent.Id);
            if (adPoint == null)
            {
                throw new ObjectNotFoundException($"AdPoint with id={deliveredEvent.Id} was not found (EventId={deliveredEvent.EventId})");
            }

            adPointRepository.Delete(adPoint);
            await commitProvider.SaveChangesAsync();
        }
    }
}

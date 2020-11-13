using AdOut.Planning.Core.EventHandlers.Base;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.EventHandlers
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
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var adPointRepository = scope.ServiceProvider.GetRequiredService<IAdPointRepository>();
                var commitProvider = scope.ServiceProvider.GetRequiredService<ICommitProvider>();

                var adPoint = await adPointRepository.GetByIdAsync(deliveredEvent.Id);
                if (adPoint == null)
                {
                    throw new ObjectNotFoundException($"Delivered AdPoint with id={deliveredEvent.Id} was not found (EventId={deliveredEvent.EventId})");
                }

                adPointRepository.Delete(adPoint);
                await commitProvider.SaveChangesAsync();
            }
        }
    }
}

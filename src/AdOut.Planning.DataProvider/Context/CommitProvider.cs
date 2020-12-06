using AdOut.Planning.Model;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Infrastructure;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Context
{
    public class CommitProvider : ICommitProvider
    {
        private readonly IDatabaseContext _context;
        private readonly IEventBroker _eventBroker;
        private readonly IMapper _mapper;

        public CommitProvider(
            IDatabaseContext context,
            IEventBroker eventBroker,
            IMapper mapper)
        {
            _context = context;
            _eventBroker = eventBroker;
            _mapper = mapper;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var integrationEvents = GenerateCRUDIntegrationEvents();
            var countChanges = await _context.SaveChangesAsync();

            //sending integration events AFTER successed execution SaveChanges to avoid no consistent data
            foreach (var integrationEvent in integrationEvents)
            {
                _eventBroker.Publish(integrationEvent);
            }

            return countChanges;
        }

        private List<IntegrationEvent> GenerateCRUDIntegrationEvents()
        {
            var integrationEvents = new List<IntegrationEvent>();

            var entries = _context.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                var entityType = entry.Entity.GetType();
                var entityStateName = ((EventReason)(int)entry.State).ToString();
                var eventName = $"{entityType.Name}{entityStateName}Event";

                var modelAssembly = typeof(Constants).Assembly;
                var eventFullName = $"{modelAssembly.GetName().Name}.Events.{eventName}";
                var eventType = modelAssembly.GetType(eventFullName);

                if (eventType != null)
                {
                    var integrationEvent = (IntegrationEvent)_mapper.Map(entry.Entity, entityType, eventType);
                    integrationEvents.Add(integrationEvent);
                }
            }

            return integrationEvents;
        }
    }
}

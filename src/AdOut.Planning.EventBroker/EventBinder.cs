using AdOut.Planning.Model.Interfaces.Infrastructure;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;

namespace AdOut.Planning.EventBroker
{
    public class EventBinder : IEventBinder
    {
        private readonly IEventBroker _eventBroker;
        private readonly IEnumerable<IBasicConsumer> _consumers;

        public EventBinder(
            IEventBroker eventBroker,
            IEnumerable<IBasicConsumer> consumers)
        {
            _eventBroker = eventBroker;
            _consumers = consumers;
        }

        public void Bind()
        {
            foreach (var consumer in _consumers)
            {
                var baseConsumerType = consumer.GetType().BaseType;
                var eventType = baseConsumerType.GetGenericArguments().Single();

                _eventBroker.Subscribe(eventType, consumer);
            }
        }
    }
}

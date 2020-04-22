using AdOut.Planning.Model.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Interfaces.Infrastructure
{
    public interface IEventBroker
    {
        void Publish(IntegrationEvent integrationEvent);
        void Subscribe(Type eventType, IBasicConsumer eventHandler);
        void Configure(IEnumerable<Type> eventTypes);
    }
}

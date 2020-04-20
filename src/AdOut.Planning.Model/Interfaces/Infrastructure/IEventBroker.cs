using AdOut.Planning.Model.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Interfaces.Infrastructure
{
    public interface IEventBroker
    {
        void Publish(IntegrationEvent integrationEvent);
        void Subscribe<TEvent>(IBasicConsumer eventHandler) where TEvent : IntegrationEvent;
        void Configure(IEnumerable<Type> eventTypes);
    }
}

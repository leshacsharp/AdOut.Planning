using AdOut.Planning.Common;
using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Infrastructure;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdOut.Planning.EventBroker
{
    public class RabbitEventBroker : IEventBroker
    {
        private readonly IEventBrokerHelper _eventBrokerHelper;
        private readonly IConnectionManager _connectionManager;

        public RabbitEventBroker(
            IEventBrokerHelper eventBrokerHelper,
            IConnectionManager connectionManager
         )
        {
            _eventBrokerHelper = eventBrokerHelper;
            _connectionManager = connectionManager;
        }

        public void Publish(IntegrationEvent integrationEvent)
        {
            var eventJson = JsonConvert.SerializeObject(integrationEvent, new TypeInfoConverter(integrationEvent.GetType()));
            var messageBody = Encoding.UTF8.GetBytes(eventJson);

            var exchange = _eventBrokerHelper.GetExchangeName(integrationEvent.GetType());
            var routingKey = _eventBrokerHelper.GetQueueName(integrationEvent.GetType());

            var channel = _connectionManager.GetPublisherChannel();
            channel.BasicPublish(exchange, routingKey, null, messageBody);

            _connectionManager.ReturnPublisherChannel(channel);
        }

        public void Subscribe(Type eventType, IBasicConsumer eventHandler)
        {
            var queue = _eventBrokerHelper.GetQueueName(eventType);

            var channel = _connectionManager.GetConsumerChannel();
            channel.BasicConsume(queue, true, eventHandler); 
        }

        public void Configure(IEnumerable<Type> eventTypes)
        {
            var channel = _connectionManager.GetPublisherChannel();

            foreach (var eventType in eventTypes)
            {
                var exchange = _eventBrokerHelper.GetExchangeName(eventType);
                var queue = _eventBrokerHelper.GetQueueName(eventType);

                channel.ExchangeDeclare(exchange, ExchangeType.Fanout);
                channel.QueueDeclare(queue, true, false, false, null);
                channel.QueueBind(queue, exchange, queue, null);
            }

            _connectionManager.ReturnPublisherChannel(channel);
        }
    }
}

using AdOut.Planning.Model.Events;
using AdOut.Planning.Model.Interfaces.Infrastructure;
using AdOut.Planning.Model.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace AdOut.Planning.EventBroker
{
    public class RabbitEventBroker : IEventBroker, IDisposable
    {
        private readonly IEventBrokerHelper _eventBrokerHelper;
        private readonly IConnection _connection;

        public RabbitEventBroker(
            IEventBrokerHelper eventBrokerHelper,
            IOptions<RabbitConnection> connectionOptions)
        {
            _eventBrokerHelper = eventBrokerHelper;

            var connectionFactory = new ConnectionFactory()
            {
                UserName = connectionOptions.Value.UserName,
                Password = connectionOptions.Value.Password,
                VirtualHost = connectionOptions.Value.VirtualHost,
                HostName = connectionOptions.Value.HostName
            };

            _connection = connectionFactory.CreateConnection();
        }

        public void Publish(IntegrationEvent integrationEvent)
        {    
            using (var channel = _connection.CreateModel())
            {
                var eventJson = JsonConvert.SerializeObject(integrationEvent, integrationEvent.GetType(), new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                var messageBody = Encoding.UTF8.GetBytes(eventJson);
                var exchange = _eventBrokerHelper.GetExchangeName(integrationEvent.GetType());
                var routingKey = _eventBrokerHelper.GetQueueName(integrationEvent.GetType());

                channel.BasicPublish(exchange, routingKey, null, messageBody);
            }
        }

        public void Subscribe<TEvent>(IBasicConsumer eventHandler) where TEvent : IntegrationEvent
        {         
            using (var channel = _connection.CreateModel())
            {
                var queue = _eventBrokerHelper.GetQueueName(typeof(TEvent));
                channel.BasicConsume(queue, true, eventHandler);
            }
        }

        public void Configure(IEnumerable<Type> eventTypes)
        {
            using (var channel = _connection.CreateModel())
            {
                foreach (var eventType in eventTypes)
                {
                    var exchange = _eventBrokerHelper.GetExchangeName(eventType);
                    var queue = _eventBrokerHelper.GetQueueName(eventType);

                    channel.ExchangeDeclare(exchange, ExchangeType.Fanout);
                    channel.QueueDeclare(queue, true, false, true, null);
                    channel.QueueBind(queue, exchange, queue, null);
                }
            }
        }

        public void Dispose()
        {
            _connection.Close();
        }  
    }
}

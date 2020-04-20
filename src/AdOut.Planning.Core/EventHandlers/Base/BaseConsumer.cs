using AdOut.Planning.Model.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.EventHandlers.Base
{
    public abstract class BaseConsumer<TEvent> : AsyncDefaultBasicConsumer where TEvent : IntegrationEvent
    {
        public override Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var jsonBody = Encoding.UTF8.GetString(body.Span);
            try
            {
                var deliveredEvent = JsonConvert.DeserializeObject<TEvent>(jsonBody, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                return HandleAsync(deliveredEvent);
            }
            catch (JsonSerializationException ex)
            {
                var exceptionMessage = $"{this.GetType().Name} received wrong type of message from (exchange={exchange}, routingKey={routingKey})";
                throw new ArgumentException(exceptionMessage, ex);
            }
        } 

        protected abstract Task HandleAsync(TEvent deliveredEvent);
    }
}

using AdOut.Extensions.Communication.Models;
using AdOut.Planning.Model.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Consumers
{
    public class TestConsumer : AsyncDefaultBasicConsumer
    {
        public async override Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var jsonBody = Encoding.UTF8.GetString(body.Span);
            var deliveredEvent = JsonConvert.DeserializeObject<ReplicationEvent<AdPoint>>(jsonBody);
            await Task.CompletedTask;
        }
    }
}

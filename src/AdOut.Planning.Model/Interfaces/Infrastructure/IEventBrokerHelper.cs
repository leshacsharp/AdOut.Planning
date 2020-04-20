using System;

namespace AdOut.Planning.Model.Interfaces.Infrastructure
{
    public interface IEventBrokerHelper
    {
        string GetQueueName(Type eventType);
        string GetExchangeName(Type eventType);
    }
}

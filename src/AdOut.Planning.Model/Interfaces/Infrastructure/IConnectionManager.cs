using RabbitMQ.Client;

namespace AdOut.Planning.Model.Interfaces.Infrastructure
{
    public interface IConnectionManager
    {
        IModel GetConsumerChannel();
        IModel GetPublisherChannel();
        void ReturnPublisherChannel(IModel channel);
    }
}

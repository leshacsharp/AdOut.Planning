using RabbitMQ.Client;

namespace AdOut.Planning.Model.Interfaces.Infrastructure
{
    public interface IChannelManager
    {
        IModel GetConsumerChannel();
        IModel GetPublisherChannel();
        void ReturnPublisherChannel(IModel channel);
    }
}

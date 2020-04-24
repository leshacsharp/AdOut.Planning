using RabbitMQ.Client;

namespace AdOut.Planning.Model.Interfaces.Infrastructure
{
    public interface IConnectionProvider
    {
        IConnection CreateConnection();
    }
}

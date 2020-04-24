using AdOut.Planning.Model.Interfaces.Infrastructure;
using AdOut.Planning.Model.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Threading;

namespace AdOut.Planning.EventBroker
{
    public class RabbitConnectionProvider : IConnectionProvider
    {
        private readonly RabbitConfig _config;
        public RabbitConnectionProvider(IOptions<RabbitConfig> config)
        {
            _config = config.Value;
        }

        public IConnection CreateConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost,
                HostName = _config.HostName
            };

            IConnection connection = null;
            var countAttemptsToConnect = 0;
            var isConnected = false;

            while (!isConnected)
            {
                try
                {
                    connection = connectionFactory.CreateConnection();
                    isConnected = true;
                }
                catch (BrokerUnreachableException)
                {
                    countAttemptsToConnect++;
                    if (countAttemptsToConnect == _config.MaxRetriesToConnect)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(_config.IntervalToConnectMs);
                    }
                }
            }

            return connection;
        }
    }
}

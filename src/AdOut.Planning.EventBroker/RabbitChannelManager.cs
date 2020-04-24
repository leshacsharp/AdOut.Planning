using AdOut.Planning.Model.Interfaces.Infrastructure;
using AdOut.Planning.Model.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.EventBroker
{
    public class RabbitChannelManager : IChannelManager, IDisposable
    {
        private readonly IConnection _connection;      
        private readonly Queue<IModel> _sharedChannels;
        private readonly object _syncDequeue = new object();
        private readonly object _syncEnqueue = new object();
        private readonly int _maxChannelsInPool;

        public RabbitChannelManager(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<RabbitConfig> config)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var connectionProvider = scope.ServiceProvider.GetRequiredService<IConnectionProvider>();
                _connection = connectionProvider.CreateConnection();
            }

            _sharedChannels = new Queue<IModel>();
            _maxChannelsInPool = config.Value.MaxChannelsInPool;
        }

        public IModel GetConsumerChannel()
        {
            return _connection.CreateModel();
        }

        public IModel GetPublisherChannel()
        {
            lock (_syncDequeue)
            {
                if (_sharedChannels.Count > 0)
                {
                    return _sharedChannels.Dequeue();
                }
                else
                {
                    return _connection.CreateModel();
                }
            }
        }

        public void ReturnPublisherChannel(IModel channel)
        {
            lock (_syncEnqueue)
            {
                if (_sharedChannels.Count < _maxChannelsInPool)
                {
                    if (channel.IsOpen)
                    {
                        _sharedChannels.Enqueue(channel);
                    }
                    else
                    {
                        channel.Dispose();
                    }
                }
            }
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}

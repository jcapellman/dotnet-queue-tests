using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;

using RESTQueue.lib.Common;

namespace RESTQueue.lib.Queue
{
    public class RabbitQueue : IQueue
    {
        private readonly IBusClient _busClient;

        public RabbitQueue(IOptions<Settings> settings) : this(settings.Value) { }

        public RabbitQueue(Settings settings)
        {
            var config = new RawRabbitConfiguration()
            {
                Port = settings.QueuePortNumber,
                Username = settings.QueueUsername,
                Password = settings.QueuePassword,
                Hostnames = new List<string> {settings.QueueHostName}
            };

            _busClient = BusClientFactory.CreateDefault(config);
        }
        
        public bool IsOnline() => _busClient != null;

        public async Task<bool> AddToQueueAsync(byte[] data, Guid guid)
        {
            try
            {
                await _busClient.PublishAsync(data, guid);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Name => "RabbitMQ";
    }
}
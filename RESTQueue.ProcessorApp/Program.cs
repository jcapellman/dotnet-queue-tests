using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Newtonsoft.Json;

using NLog;

using RawRabbit.Context;
using RawRabbit.vNext;

using RESTQueue.lib.Common;
using RESTQueue.lib.datascience;
using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;
using RESTQueue.lib.Queue;

namespace RESTQueue.ProcessorApp
{
    public class Program
    {
        private static MongoDatabase _mongoDatabase;
        private static RabbitQueue _queue;
        private static DSManager _dsManager;
        private static Logger Log = LogManager.GetCurrentClassLogger();
        
        private static List<MessageProcessor> _processors;

        static void Main(string[] args)
        {
            try
            {
                var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Constants.FILENAME_SETTINGS));

                _mongoDatabase = new MongoDatabase(settings);
                _dsManager = new DSManager();

                if (!_mongoDatabase.IsOnline())
                {
                    throw new Exception("MongoDB could not be established");
                }

                _queue = new RabbitQueue(settings);

                if (!_queue.IsOnline())
                {
                    throw new Exception("Rabbit MQ could not be established");
                }

                _processors = new List<MessageProcessor>();

                for (var x = 0; x < Environment.ProcessorCount; x++)
                {
                    _processors.Add(new MessageProcessor(_dsManager));
                }

                var busClient = BusClientFactory.CreateDefault<AdvancedMessageContext>();

                busClient.SubscribeAsync<byte[]>(SubscribeMethod);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private static async Task SubscribeMethod(byte[] data, AdvancedMessageContext context)
        {
            var processor = _processors.FirstOrDefault(a => !a.Running);

            if (processor == null)
            {
                // Keep message queued in RabbitMQ
                context.RetryLater(TimeSpan.FromSeconds(Constants.QUEUE_RETRY_SECONDS));

                return;
            }

            var isMalicious = processor.IsMalicious(data);

            var queryHashResponse = new QueryHashResponse
            {
                Guid = context.GlobalRequestId,
                Status = ResponseStatus.SCANNED,
                IsMalicious = isMalicious,
                MD5Hash = MD5.Create().ComputeHash(data).ToString()
            };

            await _mongoDatabase.Insert(queryHashResponse);
        }
    }
}
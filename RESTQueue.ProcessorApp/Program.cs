using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NLog;

using RawRabbit.Context;
using RawRabbit.vNext;

using RESTQueue.lib.CacheDAL;
using RESTQueue.lib.Common;
using RESTQueue.lib.datascience;
using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Models;

namespace RESTQueue.ProcessorApp
{
    public class Program
    {
        private static DSManager _dsManager;
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private static List<MessageProcessor> _processors;

        public static IConfiguration Configuration { get; set; }

        private static ServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();

                var services = new ServiceCollection();
                
                services.AddOptions();

                services.Configure<Settings>(Configuration.GetSection("Settings"));

                services.AddSingleton<IStorageDatabase, MongoDatabase>();
                services.AddSingleton<IStorageDatabase, LiteDBDatabase>();

                services.AddSingleton<ICache, RedisCache>();

                services.AddSingleton(typeof(StorageManager));

                _serviceProvider = services.BuildServiceProvider();

                _dsManager = new DSManager();
                
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
                // Keep message queued
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

            var insertResult = await _serviceProvider.GetService<StorageManager>().InsertAsync(queryHashResponse);

            if (!insertResult)
            {
                Log.Error("Failed to write to storage, adding back into the queue");

                context.RetryLater(TimeSpan.FromSeconds(Constants.QUEUE_RETRY_SECONDS));

                return;
            }

            if (_serviceProvider.GetService<Settings>().CacheEnabled)
            {
                var cacheResult = await _serviceProvider.GetService<ICache>().AddResponseAsync(queryHashResponse);

                if (!cacheResult)
                {
                    Log.Error($"Failed to write to {_serviceProvider.GetService<ICache>().Name} cache");
                }
            }
        }
    }
}
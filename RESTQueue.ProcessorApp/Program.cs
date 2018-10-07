﻿using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RawRabbit.Context;
using RawRabbit.vNext;

using RESTQueue.lib.Common;
using RESTQueue.lib.datascience;
using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;

namespace RESTQueue.ProcessorApp
{
    public class Program
    {
        private static MongoDatabase _mongoDatabase;
        private static DSManager _dsManager;

        static void Main(string[] args)
        {
            var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Constants.FILENAME_SETTINGS));

            _mongoDatabase = new MongoDatabase(settings);
            _dsManager = new DSManager();

            var client = BusClientFactory.CreateDefault();
            
             client.SubscribeAsync<byte[]>(SubscribeMethod);
        }

        private static async Task SubscribeMethod(byte[] data, MessageContext context)
        {
            var isMalicious = _dsManager.IsMalicious(data);
            
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
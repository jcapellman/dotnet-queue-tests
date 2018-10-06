using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

using RawRabbit.Context;
using RawRabbit.vNext;

using RESTQueue.lib.Enums;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Models;

namespace RESTQueue.ProcessorApp
{
    class Program
    {
        private static MongoDBManager _dbManager;

        static void Main(string[] args)
        {
            _dbManager = new MongoDBManager("localhost", 27017);

            var client = BusClientFactory.CreateDefault();
            
             client.SubscribeAsync<byte[]>(SubscribeMethod);
        }

        private static async Task SubscribeMethod(byte[] data, MessageContext context)
        {
            Console.WriteLine(context.GlobalRequestId);

            // TODO: Run Model
            
            var queryHashResponse = new QueryHashResponse
            {
                Guid = context.GlobalRequestId,
                Status = ResponseStatus.SCANNED,
                IsMalicious = true,
                MD5Hash = MD5.Create().ComputeHash(data).ToString()
            };

            await _dbManager.Insert(queryHashResponse);
        }
    }
}
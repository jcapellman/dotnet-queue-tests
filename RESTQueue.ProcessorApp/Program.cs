using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

using MongoDB.Driver;

using RawRabbit.Context;
using RawRabbit.vNext;

using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;

namespace RESTQueue.ProcessorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = BusClientFactory.CreateDefault();
            
             client.SubscribeAsync<byte[]>(SubscribeMethod);
        }

        private static async Task SubscribeMethod(byte[] data, MessageContext context)
        {
            Console.WriteLine(context.GlobalRequestId);

            // TODO: Run Model

            var settings = new MongoClientSettings()
            {
                Server = new MongoServerAddress("localhost", 27017)
            };

            var server = new MongoDB.Driver.MongoClient(settings);

            var db = server.GetDatabase("results");

            var collection = db.GetCollection<QueryHashResponse>("results");

            var queryHashResponse = new QueryHashResponse
            {
                Guid = context.GlobalRequestId,
                Status = ResponseStatus.SCANNED,
                IsMalicious = true,
                MD5Hash = MD5.Create().ComputeHash(data).ToString()
            };

            await collection.InsertOneAsync(queryHashResponse);
        }
    }
}
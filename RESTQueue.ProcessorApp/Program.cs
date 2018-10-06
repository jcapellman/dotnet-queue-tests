using System.Security.Cryptography;
using System.Threading.Tasks;

using RawRabbit.Context;
using RawRabbit.vNext;

using RESTQueue.lib.datascience;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Models;

namespace RESTQueue.ProcessorApp
{
    public class Program
    {
        private static MongoDBManager _dbManager;
        private static DSManager _dsManager;

        static void Main(string[] args)
        {
            _dbManager = new MongoDBManager("localhost", 27017);
            _dsManager = new DSManager();

            var client = BusClientFactory.CreateDefault();
            
             client.SubscribeAsync<byte[]>(SubscribeMethod);
        }

        private static async Task SubscribeMethod(byte[] data, MessageContext context)
        {
            var isMalicious = await _dsManager.IsMaliciousAsync(data);
            
            var queryHashResponse = new QueryHashResponse
            {
                Guid = context.GlobalRequestId,
                Status = ResponseStatus.SCANNED,
                IsMalicious = isMalicious,
                MD5Hash = MD5.Create().ComputeHash(data).ToString()
            };

            await _dbManager.Insert(queryHashResponse);
        }
    }
}
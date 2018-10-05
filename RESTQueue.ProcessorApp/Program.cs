using System;
using System.Threading.Tasks;

using RawRabbit.Context;
using RawRabbit.vNext;

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
            // TODO: Save to Database
        }
    }
}
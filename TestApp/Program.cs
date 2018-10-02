using System;
using System.Threading.Tasks;

using RESTQueue.lib.Handlers;
using RESTQueue.lib.Models;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pHandler = new PostHandler();

            for (var x = 0; x < 100; x++)
            {
                await pHandler.SubmitQueryHashAsync(new QueryHashCommand
                {
                    Hash = x.ToString().PadRight(40).ToString(),
                    Timestamp = DateTime.Now
                });
            }
        }
    }
}
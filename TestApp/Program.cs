using System;
using System.Threading.Tasks;

using RESTQueue.lib.Handlers;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pHandler = new PostHandler("http://localhost:5000/api/");

            for (var x = 0; x < 100; x++)
            {
               var result = await pHandler.SubmitQueryHashAsync(args[0]);

                Console.WriteLine(result.Guid);
            }

            Console.ReadKey();
        }
    }
}
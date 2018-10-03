using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.Handlers
{
    public class PostHandler
    {
        private static HttpClient HttpClient;

        public PostHandler()
        {
            HttpClient = new HttpClient();
        }

        public async Task SubmitQueryHashAsync(QueryHashCommand hashCommand)
        {
            var content = new StringContent(JsonConvert.SerializeObject(hashCommand), Encoding.UTF8, "application/json");

            await HttpClient.PostAsync("http://localhost:5000/api/Query", content);
        }
    }
}
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.Handlers
{
    public class PostHandler
    {
        private static HttpClient HttpClient;

        private string _baseURL;

        public PostHandler(string url)
        {
            _baseURL = url;

            HttpClient = new HttpClient();
        }

        public async Task<QueryHashResponse> SubmitQueryHashAsync(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(fileStream), "upload", fileStream.Name);

                    using (var message = await HttpClient.PostAsync($"{_baseURL}Query", content))
                    {
                        var input = await message.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<QueryHashResponse>(input);
                    }
                }
            }
        }
    }
}
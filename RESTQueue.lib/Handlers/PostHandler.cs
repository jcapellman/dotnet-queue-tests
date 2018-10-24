using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using NLog;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.Handlers
{
    public class PostHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static HttpClient _httpClient;

        private readonly string _baseUrl;

        public PostHandler(string url)
        {
            _baseUrl = url;

            _httpClient = new HttpClient();
        }

        public async Task<QueryHashResponse> SubmitQueryHashAsync(string filePath)
        {
            try
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(fileStream), "file", fileStream.Name);

                        using (var message = await _httpClient.PostAsync($"{_baseUrl}Query", content))
                        {
                            var input = await message.Content.ReadAsStringAsync();

                            return JsonConvert.DeserializeObject<QueryHashResponse>(input);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{filePath} could not be submitted");

                return null;
            }
        }
    }
}
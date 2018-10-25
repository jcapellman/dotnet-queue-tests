using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using NLog;

using RESTQueue.lib.Common;
using RESTQueue.lib.Models;

using StackExchange.Redis;

namespace RESTQueue.lib.CacheDAL
{
    public class RedisCache : ICache
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IDatabase _database;
        
        public string Name => "Redis";

        public RedisCache(IOptions<Settings> settings)
        {
            var redisConnectionMultiplexer = ConnectionMultiplexer.Connect($"{settings.Value.CacheHostName}:{settings.Value.CachePortNumber}");

            _database = redisConnectionMultiplexer.GetDatabase();
        }

        public async Task<QueryHashResponse> GetResponseAsync(string md5Hash)
        {
            try
            {
                string result = await _database.StringGetAsync(md5Hash);

                return result == RedisValue.Null ? null : JsonConvert.DeserializeObject<QueryHashResponse>(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error attempting to get {md5Hash} into {Name}");

                return null;
            }
        }

        public async Task<bool> AddResponseAsync(QueryHashResponse response)
        {
            try
            {
                return await _database.StringSetAsync(response.MD5Hash, response.ToJSON());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error attempting to save {response.MD5Hash} into {Name}");

                return false;
            }
        }
    }
}
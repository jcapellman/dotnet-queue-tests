using System;
using System.Threading.Tasks;

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

        private readonly ConnectionMultiplexer _redisConnectionMultiplexer;

        private readonly IDatabase _database;
        
        public string Name => "Redis";

        public RedisCache(Settings settings)
        {
            _redisConnectionMultiplexer = ConnectionMultiplexer.Connect(settings.CacheHostName);

            _database = _redisConnectionMultiplexer.GetDatabase();
        }

        public QueryHashResponse GetResponse(string md5Hash)
        {
            try
            {
                string result = _database.StringGet(md5Hash);

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
﻿using System;
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

        public RedisCache(IOptions<Settings> settings) : this(settings?.Value)
        {
        }

        public RedisCache(Settings settings)
        {
            try
            {
                if (settings == null)
                {
                    throw new ArgumentNullException(nameof(settings));
                }

                var redisConnectionMultiplexer = ConnectionMultiplexer.Connect($"{settings.CacheHostName}:{settings.CachePortNumber}");

                _database = redisConnectionMultiplexer.GetDatabase();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error establishing connection to Redis");

                throw;
            }
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
                if (response == null)
                {
                    throw new ArgumentNullException(nameof(response));
                }

                return await _database.StringSetAsync(response.MD5Hash, response.ToJSON());
            }
            catch (ArgumentNullException)
            {
                Log.Error("Response was null");

                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error attempting to save {response?.MD5Hash} into {Name}");

                return false;
            }
        }
    }
}
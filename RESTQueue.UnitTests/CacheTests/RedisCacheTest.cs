using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.CacheDAL;
using RESTQueue.lib.Common;
using RESTQueue.lib.Models;

namespace RESTQueue.UnitTests.CacheTests
{
    [TestClass]
    public class RedisCacheTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeNull()
        {
            var redis = new RedisCache((Settings) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeIOptionsNull()
        {
            var redis = new RedisCache((IOptions<Settings>)null);
        }
        
        private Settings RedisSettings => new Settings
        {
            CacheHostName = "localhost",
            CacheEnabled = true
        };
        
        [TestMethod]
        public async Task AddResponseAsyncNull()
        {
            var redis = new RedisCache(RedisSettings);

            await redis.AddResponseAsync(null);
        }
        
        [TestMethod]
        public async Task AddResponseAsyncEmpty()
        {
            var redis = new RedisCache(RedisSettings);

            await redis.AddResponseAsync(new QueryHashResponse());
        }

        [TestMethod]
        public async Task GetResponseNull()
        {
            var redis = new RedisCache(RedisSettings);

            await redis.GetResponseAsync(null);
        }
    }
}
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.CacheDAL;
using RESTQueue.lib.Common;

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
    }
}
using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.Common;
using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;

namespace RESTQueue.UnitTests.DALTests
{
    [TestClass]
    public class MongoDBTests
    {
        private Settings _settings;

        [TestInitialize]
        public void Setup()
        {
            _settings = new Settings
            {
                DatabaseHostName = "localhost",
                DatabasePortNumber = 27017
            };
        }

        [TestMethod]
        public async Task InsertNullTest()
        {
            var mongo = new MongoDatabase(_settings);

            var result = await mongo.Insert(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task InsertDefaultTest()
        {
            var mongo = new MongoDatabase(_settings);

            var result = await mongo.Insert(new QueryHashResponse());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task InsertGoodTest()
        {
            var mongo = new MongoDatabase(_settings);

            var response = new QueryHashResponse
            {
                Guid = Guid.NewGuid(),
                MD5Hash = "12345",
                Status = ResponseStatus.SCANNED,
                ErrorMessage = string.Empty,
                IsMalicious = false
            };

            var result = await mongo.Insert(response);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task InsertPartialGoodTest()
        {
            var mongo = new MongoDatabase(_settings);

            var response = new QueryHashResponse
            {
                Guid = Guid.NewGuid(),
                MD5Hash = string.Empty,
                Status = ResponseStatus.SCANNED,
                ErrorMessage = string.Empty,
                IsMalicious = false
            };

            var result = await mongo.Insert(response);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetGuidEmptyTask()
        {
            var mongo = new MongoDatabase(_settings);

            var result = await mongo.GetFromGUIDAsync(Guid.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IsOnlineTest()
        {
            var mongo = new MongoDatabase(_settings);

            Assert.IsTrue(mongo.IsOnline());
        }
    }
}
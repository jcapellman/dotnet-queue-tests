using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;

namespace RESTQueue.UnitTests.DALTests
{
    [TestClass]
    public class LiteDBTests
    {
        [TestMethod]
        public async Task InsertNullTest()
        {
            var liteDB = new LiteDBDatabase();

            var result = await liteDB.Insert(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task InsertDefaultTest()
        {
            var liteDB = new LiteDBDatabase();
            
            var result = await liteDB.Insert(new QueryHashResponse());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task InsertGoodTest()
        {
            var liteDB = new LiteDBDatabase();

            var queryHashResponse = new QueryHashResponse
            {
                Guid = Guid.NewGuid(),
                Status = ResponseStatus.PENDING,
                MD5Hash = "1244",
                ErrorMessage = string.Empty,
                IsMalicious = false
            };

            var result = await liteDB.Insert(queryHashResponse);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task GetFromGUIDAsync()
        {
            var liteDB = new LiteDBDatabase();

            await liteDB.GetFromGUIDAsync(Guid.Empty);
        }

        [TestMethod]
        public void IsOnline()
        {
            var liteDB = new LiteDBDatabase();

            Assert.IsTrue(liteDB.IsOnline());
        }
    }
}
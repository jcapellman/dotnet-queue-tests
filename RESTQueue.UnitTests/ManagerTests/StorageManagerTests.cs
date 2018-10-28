using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Models;

namespace RESTQueue.UnitTests.ManagerTests
{
    [TestClass]
    public class StorageManagerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullConstructor()
        {
            var storageManger = new StorageManager(null);
        }

        [TestMethod]
        public void InitializeConstructor()
        {
            var storageManger = new StorageManager(new List<IStorageDatabase>
            {
                new LiteDBDatabase()
            });
        }

        [TestMethod]
        public void GetStorageDatabaseStatusesNull()
        {
            var storageManager = new StorageManager(new List<IStorageDatabase>
            {
                new LiteDBDatabase()
            });

            var result = storageManager.GetStorageDatabaseStatuses();

            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task InsertNull()
        {
            var storageManager = new StorageManager(new List<IStorageDatabase>
            {
                new LiteDBDatabase()
            });

            var result = await storageManager.InsertAsync(null);
        }

        [TestMethod]
        public async Task InsertDefault()
        {
            var storageManager = new StorageManager(new List<IStorageDatabase>
            {
                new LiteDBDatabase()
            });

            var query = new QueryHashResponse
            {
                Guid = Guid.NewGuid(),
                MD5Hash = "1234",
                Status = ResponseStatus.PENDING,
                ErrorMessage = string.Empty,
                IsMalicious = false
            };

            var result = await storageManager.InsertAsync(query);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFromGUIDAsyncGuidEmpty()
        {
            var storageManager = new StorageManager(new List<IStorageDatabase>
            {
                new LiteDBDatabase()
            });

            var result = await storageManager.GetFromGUIDAsync(Guid.Empty);
        }
    }
}
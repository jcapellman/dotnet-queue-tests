using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Managers;

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
    }
}
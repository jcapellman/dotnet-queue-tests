using System;
using System.Collections.Generic;
using System.Linq;

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
            var storageManger = new StorageManager(new List<IStorageDatabase>
            {
                new LiteDBDatabase()
            });

            var result = storageManger.GetStorageDatabaseStatuses();

            Assert.IsNotNull(result);

            Assert.IsTrue(result.Any());
        }
    }
}
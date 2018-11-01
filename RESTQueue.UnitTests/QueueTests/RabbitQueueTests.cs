using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.Common;
using RESTQueue.lib.Queue;

namespace RESTQueue.UnitTests.QueueTests
{
    [TestClass]
    public class RabbitQueueTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeNull()
        {
            var rabbit = new RabbitQueue(settings: (Settings) null);
        }

        private Settings ValidRabbitSettings => new Settings
        {
            QueueHostName = "localhost",
            QueuePortNumber = 5672,
            QueuePassword = "guest",
            QueueUsername = "guest"
        };

        [TestMethod]
        public async Task InsertNull()
        {
            var rabbit = new RabbitQueue(ValidRabbitSettings);

            var result = await rabbit.AddToQueueAsync(null, Guid.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task InsertEmptyGuidEmpty()
        {
            var rabbit = new RabbitQueue(ValidRabbitSettings);

            var result = await rabbit.AddToQueueAsync(new byte[0], Guid.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OnlineTest()
        {
            var rabbit = new RabbitQueue(ValidRabbitSettings);

            Assert.IsTrue(rabbit.IsOnline());
        }

        [TestMethod]
        public void NameTest()
        {
            var rabbit = new RabbitQueue(ValidRabbitSettings);

            Assert.AreEqual("RabbitMQ", rabbit.Name);
        }
    }
}
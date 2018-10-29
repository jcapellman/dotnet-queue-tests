using System;

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
    }
}
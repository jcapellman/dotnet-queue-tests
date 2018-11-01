using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.Common;
using RESTQueue.lib.datascience;
using RESTQueue.ProcessorApp;

namespace RESTQueue.UnitTests.ProcessorAppTests
{
    [TestClass]
    public class MessageProcessorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitializeNull()
        {
            var messageProcessor = new MessageProcessor(null);
        }

        private static DSManager ValidDSManager => new DSManager();

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(Constants.FILENAME_MODEL, string.Empty);
        }

        [TestMethod]
        public void InitializeProperNullMalicious()
        {
            var messageProcessor = new MessageProcessor(ValidDSManager);

            messageProcessor.IsMalicious(null);
        }
    }
}
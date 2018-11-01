using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RawRabbit.Context;

using RESTQueue.ProcessorApp;

namespace RESTQueue.UnitTests.ProcessorAppTests
{
    [TestClass]
    public class ProcessorAppTests
    {
        [TestMethod]
        public void InitTest()
        {
            Program.Initialize();
        }

        [TestMethod]
        public async Task SubscribeNullNullTest()
        {
            await Program.SubscribeMethod(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SubscribeNullDataTest()
        {
            await Program.SubscribeMethod(File.ReadAllBytes(Path.GetTempFileName()), null);
        }

        [TestMethod]
        public async Task SubscribeContextDataTest()
        {
            Program.Initialize();

            await Program.SubscribeMethod(File.ReadAllBytes(Path.GetTempFileName()), new AdvancedMessageContext());
        }
    }
}
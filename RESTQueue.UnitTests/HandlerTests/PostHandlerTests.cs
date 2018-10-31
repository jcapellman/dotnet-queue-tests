using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RESTQueue.lib.Handlers;

namespace RESTQueue.UnitTests.HandlerTests
{
    [TestClass]
    public class PostHandlerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task InitializeNull()
        {
            var handler = new PostHandler(null);

            var result = await handler.SubmitQueryHashAsync(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task InitializeNullFile()
        {
            var handler = new PostHandler("localhosty");

            var result = await handler.SubmitQueryHashAsync(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task InitializeBadHost()
        {
            var handler = new PostHandler("localhosty");

            var file = Path.GetTempFileName();

            var result = await handler.SubmitQueryHashAsync(file);

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task InitializeNotFoundFile()
        {
            var handler = new PostHandler("localhosty");

            var file = @"c:\tempo\tempy";

            var result = await handler.SubmitQueryHashAsync(file);

            Assert.IsNull(result);
        }
    }
}
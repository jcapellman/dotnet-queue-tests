using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using RESTQueue.lib.Enums;
using RESTQueue.lib.Handlers;

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

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

        [TestMethod]
        [ExpectedException(typeof(JsonSerializationException))]
        public async Task InitializeAndSubmitBadResponse()
        {
            FluentMockServer server = null;

            try
            {
                server = FluentMockServer.Start(new FluentMockServerSettings {Urls = new[] {"http://+:5010"}});

                server
                    .Given(Request.Create().WithPath("/Query").UsingGet())
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithBody(@"{ ""msg"": ""Hello world!"" }")
                    );

                var handler = new PostHandler("http://localhost:5010/");

                var file = Path.GetTempFileName();

                var result = await handler.SubmitQueryHashAsync(file);

                Assert.IsNull(result);

            }
            finally
            {
                server?.Stop();
            }
        }

        [TestMethod]
        public async Task InitializeAndSubmitGoodResponse()
        {
            FluentMockServer server = null;

            try
            {
                server = FluentMockServer.Start(new FluentMockServerSettings { Urls = new[] { "http://+:5010" } });

                server
                    .Given(Request.Create().WithPath("/Query").UsingPost())
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithBody(@"{
                              ""guid"": ""00000000-0000-0000-0000-000000000000"",
                              ""status"": ""SUBMITTED"",
                              ""mD5Hash"": ""12345"",
                              ""errorMessage"": null,
                              ""isMalicious"": false
                            }")
                    );

                var handler = new PostHandler("http://localhost:5010/");

                var file = Path.GetTempFileName();

                var result = await handler.SubmitQueryHashAsync(file);

                Assert.IsNotNull(result);
                Assert.AreEqual(ResponseStatus.SUBMITTED, result.Status);
                Assert.AreEqual("12345", result.MD5Hash);
                Assert.AreEqual(false, result.IsMalicious);
                Assert.AreEqual(Guid.Empty, result.Guid);
                Assert.IsNull(result.ErrorMessage);
            }
            finally
            {
                server?.Stop();
            }
        }
    }
}
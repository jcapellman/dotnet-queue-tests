using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json;

using NLog.Extensions.Logging;

using RESTQueue.lib.CacheDAL;
using RESTQueue.lib.Common;
using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Models;
using RESTQueue.lib.Queue;

using RESTQueueAPI.Controllers;

namespace RESTQueue.UnitTests.APITests
{
    [TestClass]
    public class QueryControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNull()
        {
            var qController = new QueryController(null, null, null, null, (IOptions<Settings>) null);
        }

        private QueryController createQueryController(bool enableCache = false)
        {
            var settings =
                JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,
                    Constants.FILENAME_SETTINGS)));

            var liteDb = new LiteDBDatabase();

            ICache cache = null;
            
            if (enableCache)
            {
                cache = new RedisCache(settings);
            }

            return new QueryController(new RabbitQueue(settings), new StorageManager(new List<IStorageDatabase> { liteDb }), new NLogLoggerFactory(), cache, settings);
        }

        [TestMethod]
        public void ConstructorValidTest()
        {
            createQueryController();
        }

        [TestMethod]
        public async Task SubmitNullTest()
        {
            await createQueryController().Post(null);
        }

        [TestMethod]
        public async Task GetGuidEmptyTest()
        {
            await createQueryController().Get(Guid.Empty);
        }

        [TestMethod]
        public async Task GetGuidTest()
        {
            await createQueryController().Get(Guid.NewGuid());
        }

        [TestMethod]
        public async Task PostValidNullCacheTest()
        {
            var fileMock = new Mock<IFormFile>();

            var content = "Fake File";
            var fileName = "file.exe";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);

            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);


            await createQueryController().Post(fileMock.Object);
        }

        [TestMethod]
        public async Task PostValidCacheEnabledTest()
        {
            var fileMock = new Mock<IFormFile>();

            var content = "Fake File";
            var fileName = "file.exe";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);

            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);


            await createQueryController(true).Post(fileMock.Object);
        }

        [TestMethod]
        public async Task GetGuidValidTest()
        {
            var response = new QueryHashResponse
            {
                Guid = Guid.NewGuid(),
                Status = ResponseStatus.SUBMITTED,
                MD5Hash = "1234",
                ErrorMessage = string.Empty,
                IsMalicious = false
            };

            await new LiteDBDatabase().Insert(response);

            await createQueryController().Get(response.Guid);
        }
    }
}
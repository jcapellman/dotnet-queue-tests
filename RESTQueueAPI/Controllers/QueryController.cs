using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RESTQueue.lib.CacheDAL;
using RESTQueue.lib.Common;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Models;
using RESTQueue.lib.Queue;

namespace RESTQueueAPI.Controllers
{
    public class QueryController : BaseController
    {
        public QueryController(IQueue queue, StorageManager storageManager, ILoggerFactory logger, ICache cache,
            IOptions<Settings> settings) : base(queue, storageManager, logger, settings?.Value, cache)
        {
        }

        public QueryController(IQueue queue, StorageManager storageManager, ILoggerFactory logger, ICache cache,
            Settings settings) : base(queue, storageManager, logger, settings, cache)
        {
        }

        [HttpGet]
        public async Task<QueryHashResponse> Get(Guid guid)
        {
            try
            {
                var result = await StorageManager.GetFromGUIDAsync(guid);

                var response = result ?? new QueryHashResponse
                {
                    Guid = guid,
                    Status = ResponseStatus.PENDING
                };

                Logger.LogDebug(response.ToString());

                return response;
            }
            catch (Exception ex)
            {
                return ReturnErrorResponse(ex, guid);
            }
        }

        [HttpPost]
        public async Task<QueryHashResponse> Post(IFormFile file)
        {
            var response = new QueryHashResponse
            {
                Guid = Guid.NewGuid()
            };

            try
            {
                if (file == null)
                {
                    throw new ArgumentNullException(nameof(file));
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    if (Settings.CacheEnabled && Cache != null)
                    {
                        var md5Hash = MD5.Create().ComputeHash(memoryStream.ToArray()).ToString();

                        var cacheResult = await Cache.GetResponseAsync(md5Hash);

                        if (cacheResult != null)
                        {
                            response.Status = ResponseStatus.SCANNED;
                            response.MD5Hash = md5Hash;
                            response.IsMalicious = cacheResult.IsMalicious;

                            Logger.LogDebug(response.ToString());

                            return response;
                        }

                        Logger.LogDebug($"{Cache.Name} did not contain {md5Hash}");
                    }

                    Logger.LogDebug($"Adding {response.Guid} to the Queue");

                    await Queue.AddToQueueAsync(memoryStream.ToArray(), response.Guid);
                }

                response.Status = ResponseStatus.SUBMITTED;

                Logger.LogDebug(response.ToString());

                return response;
            }
            catch (Exception ex)
            {
                return ReturnErrorResponse(ex, response.Guid, "Failed to add to queue");
            }
        }
    }
}
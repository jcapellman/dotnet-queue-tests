using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RESTQueue.lib.CacheDAL;
using RESTQueue.lib.Common;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Managers;
using RESTQueue.lib.Models;
using RESTQueue.lib.Queue;

namespace RESTQueueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IQueue Queue;
        protected readonly StorageManager StorageManager;
        protected readonly ILogger Logger;
        protected readonly Settings Settings;
        protected readonly ICache Cache;

        public BaseController(IQueue queue, StorageManager storageManager, ILoggerFactory loggerFactory, Settings settings, ICache cache)
        {
            Queue = queue;
            StorageManager = storageManager;
            Logger = loggerFactory.CreateLogger(GetType().Namespace);
            Settings = settings;
            Cache = cache;
        }

        protected QueryHashResponse ReturnErrorResponse(Exception exception, Guid guid, string additionalError = null)
        {
            var response = new QueryHashResponse
            {
                Guid = guid,
                Status = ResponseStatus.ERROR,
                ErrorMessage = string.IsNullOrEmpty(additionalError)
                    ? exception.ToString()
                    : $"Exception: {exception} | Additional Information: {additionalError}"
            };

            Logger.LogError(response.ToString());

            return response;
        }
    }
}
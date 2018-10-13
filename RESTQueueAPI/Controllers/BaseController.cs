using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;
using RESTQueue.lib.Queue;

namespace RESTQueueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IQueue Queue;
        protected readonly IStorageDatabase Database;
        protected readonly ILogger Logger;
        
        public BaseController(IQueue queue, IStorageDatabase database, ILoggerFactory loggerFactory)
        {
            Queue = queue;
            Database = database;
            Logger = loggerFactory.CreateLogger(GetType().Namespace);
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
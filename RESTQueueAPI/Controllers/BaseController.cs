using System;

using Microsoft.AspNetCore.Mvc;

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

        protected NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public BaseController(IQueue queue, IStorageDatabase database)
        {
            Queue = queue;
            Database = database;
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

            Logger.Error(response.ToString());

            return response;
        }
    }
}
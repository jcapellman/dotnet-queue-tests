using System;

using Microsoft.AspNetCore.Mvc;

using RawRabbit;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;

namespace RESTQueueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IBusClient Bus;
        protected readonly IStorageDatabase Database;

        protected NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public BaseController(IBusClient bus, IStorageDatabase database)
        {
            Bus = bus;
            Database = database;
        }

        protected QueryHashResponse ReturnErroResponse(Exception exception, Guid guid, string additionalError = null)
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
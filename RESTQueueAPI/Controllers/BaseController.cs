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
        protected readonly IBusClient _bus;
        protected readonly IStorageDatabase _database;

        public BaseController(IBusClient bus, IStorageDatabase database)
        {
            _bus = bus;
            _database = database;
        }

        protected QueryHashResponse ReturnErroResponse(Exception exception, Guid guid, string additionalError = null) => new QueryHashResponse
        {
            Guid = guid,
            Status = ResponseStatus.ERROR,
            ErrorMessage = string.IsNullOrEmpty(additionalError) ? exception.ToString() : $"Exception: {exception} | Additional Information: {additionalError}"
        };
    }
}
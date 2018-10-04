using System;

using Microsoft.AspNetCore.Mvc;

using RawRabbit;

using RESTQueue.lib.Models;

namespace RESTQueueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IBusClient _bus;

        public QueryController(IBusClient bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public ResultResponse Get(Guid guid)
        {
            return new ResultResponse(); 
        }
        
        [HttpPost]
        public QueryHashResponse Post(QueryHashCommand queryHash)
        {
            var response = new QueryHashResponse
            {
                Guid = Guid.NewGuid()
            };

            _bus.PublishAsync(queryHash, response.Guid);

            return response;
        }
    }
}
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
        
        [HttpPost]
        public void Post(QueryHashCommand queryHash)
        {
            _bus.PublishAsync(queryHash);
        }
    }
}
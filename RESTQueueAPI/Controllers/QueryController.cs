using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using RawRabbit;

using RESTQueue.lib.Enums;
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
        public QueryHashResponse Get(Guid guid)
        {
            return new QueryHashResponse();
        }
        
        [HttpPost]
        public async Task<QueryHashResponse> Post()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return new QueryHashResponse
                {
                    Status = ResponseStatus.ERROR
                };
            }

            var response = new QueryHashResponse
            {
                Guid = Guid.NewGuid()
            };

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);

                var bytes = new byte[file.Length];

                stream.Read(bytes);
                
                await _bus.PublishAsync(bytes, response.Guid);
            }
            
            return response;
        }
    }
}
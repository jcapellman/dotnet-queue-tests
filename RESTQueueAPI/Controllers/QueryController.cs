using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Enums;
using RESTQueue.lib.Models;
using RESTQueue.lib.Queue;

namespace RESTQueueAPI.Controllers
{
    public class QueryController : BaseController
    {
        public QueryController(IQueue queue, IStorageDatabase database) : base(queue, database) { }
    
        [HttpGet]
        public async Task<QueryHashResponse> Get(Guid guid)
        {
            try
            {
                var result = await Database.GetFromGUIDAsync(guid);
                
                var response = result ?? new QueryHashResponse
                {
                    Guid = guid,
                    Status = ResponseStatus.PENDING
                };

                Logger.Debug(response.ToString());

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

                    await Queue.AddToQueueAsync(memoryStream.ToArray(), response.Guid);
                }
            
                response.Status = ResponseStatus.SUBMITTED;

                Logger.Debug(response.ToString());

                return response;
            }
            catch (Exception ex)
            {
                return ReturnErrorResponse(ex, response.Guid, "Failed to add to queue");
            }
        }
    }
}
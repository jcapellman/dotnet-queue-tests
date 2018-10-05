﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;

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
        public async Task<QueryHashResponse> Get(Guid guid)
        {
            var settings = new MongoClientSettings()
            {
                Server = new MongoServerAddress("localhost", 27017)
            };

            var server = new MongoDB.Driver.MongoClient(settings);

            var db = server.GetDatabase("results");

            var collection = db.GetCollection<QueryHashResponse>("results");

            var result = await collection.FindAsync(a => a.Guid == guid);

            if (result == null)
            {
                return new QueryHashResponse
                {
                    Guid = guid,
                    Status = ResponseStatus.PENDING
                };
            }

            return result.Current.FirstOrDefault();
        }
        
        [HttpPost]
        public async Task<QueryHashResponse> Post()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return new QueryHashResponse
                {
                    Status = ResponseStatus.ERROR,
                    ErrorMessage = "Failed to upload file"
                };
            }

            var response = new QueryHashResponse
            {
                Guid = Guid.NewGuid(),
                Status = ResponseStatus.SUBMITTED
            };

            try
            {
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    var bytes = new byte[file.Length];

                    stream.Read(bytes);

                    await _bus.PublishAsync(bytes, response.Guid);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Failed to upload to queue {ex}";
                response.Status = ResponseStatus.ERROR;
            }

            return response;
        }
    }
}
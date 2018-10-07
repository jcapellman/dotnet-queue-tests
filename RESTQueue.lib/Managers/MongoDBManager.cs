using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using RESTQueue.lib.Common;
using RESTQueue.lib.Models;

namespace RESTQueue.lib.Managers
{
    public class MongoDBManager
    {
        private readonly IMongoDatabase _db;

        public MongoDBManager(IOptions<Settings> settings) : this(settings.Value) { }

        public MongoDBManager(Settings settings)
        {
            var mongoSettings = new MongoClientSettings()
            {
                Server = new MongoServerAddress(settings.MongoHostName, settings.MongoPortNumber)
            };

            var client = new MongoClient(mongoSettings);

            _db = client.GetDatabase("results");
        }

        public async Task<IAsyncCursor<QueryHashResponse>> GetFromGUIDAsync(Guid guid)
        {
            var collection = _db.GetCollection<QueryHashResponse>("results");

            return await collection.FindAsync(a => a.Guid == guid);
        }

        public async Task Insert(QueryHashResponse item)
        {
            var collection = _db.GetCollection<QueryHashResponse>("results");
            
            await collection.InsertOneAsync(item);
        }
    }
}
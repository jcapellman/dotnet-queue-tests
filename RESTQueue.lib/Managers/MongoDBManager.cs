using System;
using System.Threading.Tasks;

using MongoDB.Driver;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.Managers
{
    public class MongoDBManager
    {
        private MongoClient _client;
        private IMongoDatabase _db;

        public MongoDBManager(string host, int port)
        {
            var settings = new MongoClientSettings()
            {
                Server = new MongoServerAddress(host, port)
            };

            _client = new MongoClient(settings);

            _db = _client.GetDatabase("results");
        }

        public async Task<IAsyncCursor<QueryHashResponse>> GetFromGUIDAsync(Guid guid)
        {
            var collection = _db.GetCollection<QueryHashResponse>("results");

            return await collection.FindAsync(a => a.Guid == guid);
        }
    }
}
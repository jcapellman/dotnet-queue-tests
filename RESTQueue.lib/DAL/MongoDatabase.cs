using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

using RESTQueue.lib.Common;
using RESTQueue.lib.Models;

namespace RESTQueue.lib.DAL
{
    public class MongoDatabase : IStorageDatabase
    {
        private readonly IMongoDatabase _db;

        public MongoDatabase(IOptions<Settings> settings) : this(settings.Value) { }

        public MongoDatabase(Settings settings)
        {
            var mongoSettings = new MongoClientSettings()
            {
                Server = new MongoServerAddress(settings.MongoHostName, settings.MongoPortNumber)
            };

            var client = new MongoClient(mongoSettings);

            _db = client.GetDatabase("results");
        }

        public async Task<QueryHashResponse> GetFromGUIDAsync(Guid guid)
        {
            var collection = _db.GetCollection<QueryHashResponse>("results");

            return (await collection.FindAsync(a => a.Guid == guid)).FirstOrDefault();
        }

        public async Task Insert(QueryHashResponse item)
        {
            var collection = _db.GetCollection<QueryHashResponse>("results");

            await collection.InsertOneAsync(item);
        }

        public bool IsOnline() => _db.Client.Cluster.Description.State == ClusterState.Connected;
    }
}
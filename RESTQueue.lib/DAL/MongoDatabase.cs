using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using NLog;

using RESTQueue.lib.Common;
using RESTQueue.lib.Models;

namespace RESTQueue.lib.DAL
{
    public class MongoDatabase : IStorageDatabase
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IMongoDatabase _db;

        public MongoDatabase(IOptions<Settings> settings) : this(settings.Value) { }
        
        public MongoDatabase(Settings settings)
        {
            var mongoSettings = new MongoClientSettings()
            {
                Server = new MongoServerAddress(settings.DatabaseHostName, settings.DatabasePortNumber)
            };

            var client = new MongoClient(mongoSettings);

            _db = client.GetDatabase("results");
        }

        public async Task<QueryHashResponse> GetFromGUIDAsync(Guid guid)
        {
            var collection = _db.GetCollection<QueryHashResponse>("results");

            return (await collection.FindAsync(a => a.Guid == guid)).FirstOrDefault();
        }

        public async Task<bool> Insert(QueryHashResponse item)
        {
            try
            {
                var collection = _db.GetCollection<QueryHashResponse>("results");

                await collection.InsertOneAsync(item);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in {Name} when attempting to write {item}");

                return false;
            }
        }

        public bool IsOnline() => _db.Client.StartSession().ServerSession.Id != null;

        public string Name => "MongoDB";
    }
}
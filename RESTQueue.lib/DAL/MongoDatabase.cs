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
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IMongoDatabase _db;

        public string Name => "MongoDB";

        /// <summary>
        /// Initializes the Settings into the MongoDB
        /// </summary>
        /// <param name="settings">Settings of the App</param>
        public MongoDatabase(IOptions<Settings> settings) : this(settings.Value) { }

        /// <summary>
        /// Initializes the Settings into the MongoDB
        /// </summary>
        /// <param name="settings">Settings of the App</param>
        public MongoDatabase(Settings settings)
        {
            try
            {
                var mongoSettings = new MongoClientSettings()
                {
                    Server = new MongoServerAddress(settings.DatabaseHostName, settings.DatabasePortNumber)
                };

                var client = new MongoClient(mongoSettings);

                _db = client.GetDatabase("results");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Initializing {Name}");
            }
        }

        /// <summary>
        /// Retrieves the Query Hash Response given the Guid from MongoDB
        /// </summary>
        /// <param name="guid">Guid of the response</param>
        /// <returns>Null if not found or the response</returns>
        public async Task<QueryHashResponse> GetFromGUIDAsync(Guid guid)
        {
            try
            {
                var collection = _db.GetCollection<QueryHashResponse>("results");

                return (await collection.FindAsync(a => a.Guid == guid)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Attempting to get {guid} from {Name}");

                return null;
            }
        }

        /// <summary>
        /// Inserts the QueryHashResponse item into the MongoDB
        /// </summary>
        /// <param name="item">QueryHashResponse to be added to the database</param>
        /// <returns>True if successfull, false otherwise</returns>
        public async Task<bool> Insert(QueryHashResponse item)
        {
            try
            {
                if (string.IsNullOrEmpty(item.MD5Hash))
                {
                    throw new ArgumentNullException(nameof(item.MD5Hash));
                }

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

        /// <summary>
        /// Checks the connection status of MongoDB
        /// </summary>
        /// <returns>True if online, false otherwise</returns>
        public bool IsOnline() => _db?.Client?.StartSession()?.ServerSession.Id != null;
    }
}
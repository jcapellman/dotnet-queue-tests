using System;
using System.Threading.Tasks;

using NLog;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.DAL
{
    public class LiteDBDatabase : IStorageDatabase
    {
        private const string DB_FILE_NAME = "litedb.db";

        private static Logger Log = LogManager.GetCurrentClassLogger();
        
        public Task<QueryHashResponse> GetFromGUIDAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(QueryHashResponse item)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (item == null)
                    {
                        throw new ArgumentNullException(nameof(item));
                    }

                    using (var db = new LiteDB.LiteDatabase(DB_FILE_NAME))
                    {
                        var collection = db.GetCollection<QueryHashResponse>();

                        return collection.Insert(item) > 0;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Attempting to insert {item} into {Name}");

                    return false;
                }
            });
        }

        public bool IsOnline()
        {
            // TODO: Do a better job here
            return true;
        }

        public string Name => "LiteDB";
    }
}
using System;
using System.Threading.Tasks;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.DAL
{
    public class LiteDBDatabase : IStorageDatabase
    {
        private const string DB_FILE_NAME = "litedb.db";

        public Task<QueryHashResponse> GetFromGUIDAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(QueryHashResponse item)
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDB.LiteDatabase(DB_FILE_NAME))
                {
                    var collection = db.GetCollection<QueryHashResponse>();

                    collection.Insert(item);
                }
            });
        }

        public bool IsOnline()
        {
            return true;
        }

        public string Name => "LiteDB";
    }
}
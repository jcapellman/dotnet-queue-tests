using System.Collections.Generic;
using System.Threading.Tasks;

using NLog;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Models;

namespace RESTQueue.lib.Managers
{
    public class StorageManager
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private List<IStorageDatabase> _storageDatabases;

        public bool InitializeStorage(params IStorageDatabase[] databases)
        {
            _storageDatabases = new List<IStorageDatabase>(databases.Length);

            foreach (var database in databases)
            {
                _storageDatabases.Add(database);
            }

            return true;
        }

        public async Task<bool> InsertAsync(QueryHashResponse response)
        {
            foreach (var database in _storageDatabases)
            {
                var result = await database.Insert(response);

                if (result)
                {
                    Log.Debug($"{database.Name} inserted {response} successfully");

                    return true;
                }
            }

            Log.Warn($"Could not log {response} in any storage device");

            return false;
        }
    }
}

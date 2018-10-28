using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NLog;

using RESTQueue.lib.DAL;
using RESTQueue.lib.Models;

namespace RESTQueue.lib.Managers
{
    public class StorageManager
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly List<IStorageDatabase> _storageDatabases;

        public StorageManager(IEnumerable<IStorageDatabase> databases)
        {
            if (databases == null)
            {
                throw new ArgumentNullException(nameof(databases));
            }

            _storageDatabases = new List<IStorageDatabase>();

            foreach (var database in databases)
            {
                _storageDatabases.Add(database);
            }
        }

        public IEnumerable<(string Name, bool IsOnline)> GetStorageDatabaseStatuses() => _storageDatabases.Select(a => (a.Name, a.IsOnline()));

        public async Task<QueryHashResponse> GetFromGUIDAsync(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException($"Guid is equal to {guid}");
            }

            foreach (var database in _storageDatabases)
            {
                var result = await database.GetFromGUIDAsync(guid);

                if (result == null)
                {
                    Log.Debug($"{database.Name} did not get a match on {guid}");

                    continue;
                }

                Log.Debug($"{database.Name} obtained {guid} successfully {result}");

                return result;
            }

            Log.Warn($"Could not retreieve {guid} in any storage device");

            return null;
        }

        public async Task<bool> InsertAsync(QueryHashResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            foreach (var database in _storageDatabases)
            {
                var result = await database.Insert(response);

                if (!result)
                {
                    continue;
                }

                Log.Debug($"{database.Name} inserted {response} successfully");

                return true;
            }

            Log.Warn($"Could not log {response} in any storage device");

            return false;
        }
    }
}
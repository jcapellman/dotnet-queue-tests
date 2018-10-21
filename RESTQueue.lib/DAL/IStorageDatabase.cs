using System;
using System.Threading.Tasks;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.DAL
{
    public interface IStorageDatabase
    {
        Task<QueryHashResponse> GetFromGUIDAsync(Guid guid);

        Task<bool> Insert(QueryHashResponse item);

        bool IsOnline();

        string Name { get; }
    }
}
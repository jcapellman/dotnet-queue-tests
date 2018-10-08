using System;
using System.Threading.Tasks;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.DAL
{
    public interface IStorageDatabase
    {
        Task<QueryHashResponse> GetFromGUIDAsync(Guid guid);

        Task Insert(QueryHashResponse item);

        bool IsOnline();
    }
}
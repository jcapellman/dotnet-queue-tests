using System.Threading.Tasks;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.CacheDAL
{
    public interface ICache
    {
        QueryHashResponse GetResponse(string md5Hash);

        Task<bool> AddResponseAsync(QueryHashResponse response);

        string Name { get; }
    }
}
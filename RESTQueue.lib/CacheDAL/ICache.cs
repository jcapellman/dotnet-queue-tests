using System.Threading.Tasks;

using RESTQueue.lib.Models;

namespace RESTQueue.lib.CacheDAL
{
    public interface ICache
    {
        Task<QueryHashResponse> GetResponseAsync(string md5Hash);
        
        Task<bool> AddResponseAsync(QueryHashResponse response);

        string Name { get; }
    }
}
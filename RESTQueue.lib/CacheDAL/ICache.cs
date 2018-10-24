using RESTQueue.lib.Models;

namespace RESTQueue.lib.CacheDAL
{
    public interface ICache
    {
        QueryHashResponse GetResponse(string md5Hash);

        bool AddResponse(QueryHashResponse response);
    }
}
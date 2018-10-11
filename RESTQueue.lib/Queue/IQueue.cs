using System;
using System.Threading.Tasks;

namespace RESTQueue.lib.Queue
{
    public interface IQueue
    {
        bool IsOnline();

        Task<bool> AddToQueueAsync(byte[] data, Guid guid);

        string Name { get; }
    }
}
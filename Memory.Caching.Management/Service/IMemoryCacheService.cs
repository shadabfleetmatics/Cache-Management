// Copyright © 2023.

using System.Threading.Tasks;
using Memory.Caching.Management.Repository;

namespace Memory.Caching.Management.Service
{

    public interface IMemoryCacheService
    {
        IMemoryCacheRepository CacheRepository { get; }

        void Add<T>(string key, T item);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Remove(string key);
    }
}

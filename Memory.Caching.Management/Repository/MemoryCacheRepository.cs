// Copyright © 2023.


using Memory.Caching.Management.Service;

namespace Memory.Caching.Management.Repository
{
    public class MemoryCacheRepository : IMemoryCacheRepository
    {
        internal readonly MemoryCacheCore cache;

        public MemoryCacheRepository()
        {
            cache =  MemoryCacheCore.Instance;
        }

        public void Set(string key, object value)
        {
            cache.Add(key, value);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public void Dispose()
        {
            cache?.Dispose();
        }
    }
}

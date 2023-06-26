// Copyright © 2023.

using System;


namespace Memory.Caching.Management.Repository
{
    public interface IMemoryCacheRepository : IDisposable
    {
        void Set(string key, object item);
        object Get(string key);
        void Remove(string key);
    }
}

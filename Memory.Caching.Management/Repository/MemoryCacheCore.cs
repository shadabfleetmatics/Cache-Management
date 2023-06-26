// Copyright © 2023.

using System;
using System.Collections.Generic;

namespace Memory.Caching.Management.Service
{
    public class MemoryCacheCore : IDisposable
    {
        private readonly int maxCapacity;
        private readonly Dictionary<string, LinkedListNode<CacheItem>> cacheMap;
        private readonly LinkedList<CacheItem> cacheList;

        private static MemoryCacheCore instance;
        private static readonly object lockObject = new object();

        public static MemoryCacheCore Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new MemoryCacheCore();
                        }
                    }
                }
                return instance;
            }
        }

        private MemoryCacheCore()
        {
            maxCapacity = 100; // Set desired maximum capacity here
            cacheMap = new Dictionary<string, LinkedListNode<CacheItem>>(maxCapacity);
            cacheList = new LinkedList<CacheItem>();
        }

        public object Get(string key)
        {
            lock (lockObject)
            {
                if (cacheMap.TryGetValue(key, out var node))
                {
                    object value = node.Value.Value;
                    cacheList.Remove(node);
                    cacheList.AddFirst(node);
                    return value;
                }
                return default(object);
            }
        }

        public void Add(string key, object value)
        {
            lock (lockObject)
            {
                if (cacheMap.Count >= maxCapacity)
                {
                    Evict();
                }

                CacheItem cacheItem = new CacheItem(key, value);
                LinkedListNode<CacheItem> node = new LinkedListNode<CacheItem>(cacheItem);
                cacheList.AddFirst(node);
                cacheMap[key] = node;
            }
        }

        public void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            lock (lockObject)
            {
                if (cacheMap.TryGetValue(key, out var node))
                {
                    cacheList.Remove(node);
                    cacheMap.Remove(node.Value.Key);
                }
            }

        }


        private void Evict()
        {
            LinkedListNode<CacheItem> lastNode = cacheList.Last;
            cacheMap.Remove(lastNode.Value.Key);
            cacheList.RemoveLast();
        }

        public void Dispose()
        {
            this.Dispose();
        }

        private class CacheItem
        {
            public string Key { get; }
            public object Value { get; }

            public CacheItem(string key, object value)
            {
                Key = key;
                Value = value;
            }

        }
    }
}

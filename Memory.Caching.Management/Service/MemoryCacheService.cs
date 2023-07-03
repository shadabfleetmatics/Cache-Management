// Copyright © 2023.

using System;
using System.Threading.Tasks;
using Memory.Caching.Management.Repository;

namespace Memory.Caching.Management.Service
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly Lazy<IMemoryCacheRepository> memoryCacheRepository;

        public MemoryCacheService() :  this(DefaultCacheProvider)
        {
        }

        public MemoryCacheService(Lazy<IMemoryCacheRepository> memoryCacheRepository)
        {
            this.memoryCacheRepository = memoryCacheRepository ?? throw new ArgumentNullException(nameof(memoryCacheRepository));
        }

        public MemoryCacheService(Func<IMemoryCacheRepository> cacheProviderFactory)
        {
            if (cacheProviderFactory == null) throw new ArgumentNullException(nameof(cacheProviderFactory));
            memoryCacheRepository = new Lazy<IMemoryCacheRepository>(cacheProviderFactory);

        }

        public MemoryCacheService(IMemoryCacheRepository cache) : this(() => cache)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));
        }

        public static Lazy<IMemoryCacheRepository> DefaultCacheProvider { get; set; }
            = new Lazy<IMemoryCacheRepository>(() =>
                new MemoryCacheRepository());



        public virtual void Add<T>(string key, T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);

            CacheRepository.Set(key, item);
        }

        public virtual T Get<T>(string key)
        {
            ValidateKey(key);

            var item = CacheRepository.Get(key);

            return GetValueFromLazy<T>(item, out _);
        }

        public virtual Task<T> GetAsync<T>(string key)
        {
            ValidateKey(key);

            var item = CacheRepository.Get(key);

            return GetValueFromAsyncLazy<T>(item, out _);
        }


        public virtual void Remove(string key)
        {
            ValidateKey(key);
            CacheRepository.Remove(key);
        }

        public virtual IMemoryCacheRepository CacheRepository => memoryCacheRepository.Value;


        protected virtual T GetValueFromLazy<T>(object item, out bool valueHasChangedType)
        {
            valueHasChangedType = false;
            switch (item)
            {
                case Lazy<T> lazy:
                    return lazy.Value;
                case T variable:
                    return variable;
                case Task<T> task:
                    return task.Result;
            }

            var itemsType = item?.GetType();
            if (itemsType != null && itemsType.IsGenericType && itemsType.GetGenericTypeDefinition() == typeof(Lazy<>))
            {
                valueHasChangedType = true;
            }

            return default(T);
        }

        protected virtual Task<T> GetValueFromAsyncLazy<T>(object item, out bool valueHasChangedType)
        {
            valueHasChangedType = false;
            switch (item)
            {
                case Task<T> task:
                    return task;
                case Lazy<T> lazy:
                    return Task.FromResult(lazy.Value);
                case T variable:
                    return Task.FromResult(variable);
            }

            var itemsType = item?.GetType();
            if (itemsType != null && itemsType.IsGenericType)
            {
                valueHasChangedType = true;
            }

            return Task.FromResult(default(T));
        }


        protected virtual void ValidateKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentOutOfRangeException(nameof(key), "keys can not be null or empty");
        }
    }
}

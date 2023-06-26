// Copyright © 2023.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Memory.Caching.Management.Repository;
using Memory.Caching.Management.Service;

namespace Memory.Caching.Management
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddMemoryCacheService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCacheRepository, MemoryCacheRepository>());
            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCacheService, MemoryCacheService>(serviceProvider =>
                new MemoryCacheService(
                    new Lazy<IMemoryCacheRepository>(serviceProvider.GetRequiredService<MemoryCacheRepository>))));

            return services;
        }

        public static IServiceCollection AddMemoryCacheService(this IServiceCollection services,
            Func<IServiceProvider, MemoryCacheService> implementationFactory)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCacheRepository, MemoryCacheRepository>());
            services.TryAdd(ServiceDescriptor.Singleton<IMemoryCacheService>(implementationFactory));

            return services;
        }

    }
}


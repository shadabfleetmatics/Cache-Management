// Copyright © 2023.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Memory.Caching.Management
{
    public class AsyncLazyService<T> : Lazy<Task<T>>
    {
        public AsyncLazyService(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        public AsyncLazyService(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        {
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }
    }
}

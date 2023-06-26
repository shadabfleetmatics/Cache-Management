# VerizonConnect.Reveal.Authorization.TokenManagement

Memory cache is a simple in-memory caching service. It provides a thread safe cache implementation that guarantees to only execute your cachable delegates once (it's lazy!). Under the hood it leverages  Lazy to provide performance and reliability in heavy load scenarios.

Download

## How to use it
Add the MemoryCache services in your aspnet core Startup.cs

 // This method gets called by the runtime. Use this method to add services.
public void ConfigureServices(IServiceCollection services)
{
    // already existing registrations
   
    ....

    // Register MemoryCachee - makes the IMemoryCacheService implementation
    // MemoryCachingService available to your code
    services.AddMemoryCacheService();
}



Create a memory cache an use it:

//Create my cache manually
IMemoryCacheService cache = new CachingService();

...

//  cache the results
var productsWithCaching = cache.Add("mykey", myvalue);

// use the cached results

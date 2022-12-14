using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache
{
    public static class Cache
    {
        private static readonly IMemoryCache _cache = new MemoryCache(
            new MemoryCacheOptions
            {
                SizeLimit = 1024
            });

        public static T getItemFromCache<T>(string key, Func<T> itemSettingFunc)
        {
            T cacheItem;
            _cache.TryGetValue<T>(key, out cacheItem);
            if(cacheItem == null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal);
                cacheItem = itemSettingFunc();
                _cache.Set(key, cacheItem, cacheEntryOptions);
            }
            return cacheItem;
        }

    }
}

using Microsoft.Extensions.Caching.Memory;

namespace Server.Extensions
{
    public static class CacheExtension
    {
        public static T Remember<T>(this IMemoryCache cache, string key, Func<T> func) where T : class
        {
            if (cache.TryGetValue<T>(key, out var result))
            {
                return result;
            }

            result = func();
            cache.Set(key, result);
            return result;
        }
    }
}
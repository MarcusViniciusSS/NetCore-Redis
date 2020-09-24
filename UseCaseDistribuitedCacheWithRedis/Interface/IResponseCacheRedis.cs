using System;
using System.Threading.Tasks;

namespace UseCaseDistribuitedCacheWithRedis.Interface
{
    interface IResponseCacheRedis
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
        Task RemoveCachedResponseAsync(string cacheKey);
    }
}

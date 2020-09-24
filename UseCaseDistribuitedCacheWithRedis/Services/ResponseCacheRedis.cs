using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UseCaseDistribuitedCacheWithRedis.Interface;

namespace UseCaseDistribuitedCacheWithRedis.Services
{
    public class ResponseCacheRedis : IResponseCacheRedis
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheRedis(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive)
        {
            if (response == null)
            {
                return;
            }

            var serializedResponse = JsonSerializer.Serialize(response);

            await _distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
               AbsoluteExpirationRelativeToNow = timeTimeLive
            });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);

            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }

        public async Task RemoveCachedResponseAsync(string cacheKey)
        {
            await _distributedCache.RemoveAsync(cacheKey);
        }
    }
}

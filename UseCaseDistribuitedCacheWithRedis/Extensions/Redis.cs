using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UseCaseDistribuitedCacheWithRedis.Interface;
using UseCaseDistribuitedCacheWithRedis.Services;

namespace UseCaseDistribuitedCacheWithRedis.Extensions
{
    public static class Redis
    {
        private static readonly string SectionName = "redis";

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = configuration.GetSection(SectionName);

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration["ConnectionString"]));
            services.AddStackExchangeRedisCache(options => options.Configuration = redisConfiguration["ConnectionString"]);
            services.AddSingleton<IResponseCacheRedis, ResponseCacheRedis>();

            return services;
        }
    }
}

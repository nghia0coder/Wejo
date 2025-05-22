using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Wejo.Identity.Application.Extensions;

public static class DiRedisExtensions
{
    /// <summary>
    /// Add DI for Redis using StackExchange.Redis
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Updated service collection</returns>
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        // Retrieve Redis configuration
        var host = configuration["Redis:Host"] ?? "redis";
        var port = configuration["Redis:Port"] ?? "6379";
        var connectionString = $"{host}:{port}";

        // Configure Redis cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        });

        // Register IConnectionMultiplexer as a singleton
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));

        return services;
    }
}
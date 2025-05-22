using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace Wejo.Identity.Application.Services;

using Commands;
using Common.Domain.Database;
using Common.Domain.Dtos;
using Interfaces;

public class UserCacheService : BaseH, IUserCacheService
{
    private readonly IDatabase _redisDb;

    public UserCacheService(IConnectionMultiplexer redis, WejoContext context) : base(context)
    {
        _redisDb = redis.GetDatabase();
    }
    public async Task<UserInfoDto> GetUserInfoAsync(string userId, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:info:{userId}";
        var cachedData = await _redisDb.StringGetAsync(cacheKey);

        if (cachedData.HasValue)
        {
            return JsonSerializer.Deserialize<UserInfoDto>(cachedData);
        }

        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto
            {
                FullName = u.FirstName + " " + u.LastName,
                Avartar = u.Avatar ?? "default-avatar.png"
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return new UserInfoDto
            {
                FullName = "Unknown",
                Avartar = "default-avatar.png"
            };
        }

        await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(user), TimeSpan.FromHours(24));
        return user;
    }

    public async Task SetUserInfoAsync(string userId, UserInfoDto userInfo, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:info:{userId}";
        await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(userInfo), TimeSpan.FromHours(24));
    }

    public async Task RemoveUserInfoAsync(string userId, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:info:{userId}";
        await _redisDb.KeyDeleteAsync(cacheKey);
    }
}


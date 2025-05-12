namespace Wejo.Identity.Application.Interfaces;

using Common.Domain.Dtos;

public interface IUserCacheService
{
    Task<UserInfoDto> GetUserInfoAsync(string userId, CancellationToken cancellationToken);
    Task SetUserInfoAsync(string userId, UserInfoDto userInfo, CancellationToken cancellationToken);
    Task RemoveUserInfoAsync(string userId, CancellationToken cancellationToken);
}

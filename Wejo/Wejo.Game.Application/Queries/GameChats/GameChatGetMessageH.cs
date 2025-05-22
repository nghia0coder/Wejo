using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Queries;

using Common.Core.Enums;
using Common.Core.Extensions;
using Common.Domain.Dtos;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Interfaces;
using Request;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class GameChatGetMessageH : BaseH, IRequestHandler<GameChatGetMessageR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameChatGetMessageH(IWejoContext context, IGameChatService gameChatService, IUserCacheService userCacheService) : base(context)
    {
        _gameChatService = gameChatService;
        _userCacheService = userCacheService;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(GameChatGetMessageR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        #region -- Validate on client --
        var vr = new GameChatGetMessageV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }
        #endregion

        var userId = request.UserId;
        if (userId == null)
        {
            return res.SetError(nameof(E119), E119);
        }

        #region -- Validate on server --
        var hasUser = await _context.Users.AnyAsync(p => p.Id == userId, cancellationToken);
        if (!hasUser)
        {
            var t = new List<DicDto> { new() { Key = nameof(userId).ToCamelCase(), Value = userId } };
            return res.SetError(nameof(E119), E119, t);
        }
        var hasParticipant = await _context.GameParticipants.AnyAsync(p => p.GameId == request.Id &&
                                                                      p.UserId == request.UserId &&
                                                                      p.Status == PlayerStatus.Accepted, cancellationToken);
        if (!hasParticipant)
        {
            var t = new List<DicDto> { new() { Key = nameof(request.Id).ToCamelCase(), Value = request.Id } };
            return res.SetErrorData(nameof(E205), E205, t);
        }
        #endregion

        var gameId = request.Id;

        var messages = await _gameChatService.GetMessagesAsync(gameId, request.Before, request.After, request.FromUser,
            request.Limit, cancellationToken);

        var userIds = messages.Select(m => m.UserId).Distinct().ToList();
        var users = new Dictionary<string, UserInfoDto>();

        foreach (var uid in userIds)
        {
            var userInfo = await _userCacheService.GetUserInfoAsync(uid, cancellationToken);
            users[uid] = userInfo;
        }

        var (lastReadMessageId, lastReadTimestamp) = await _gameChatService.GetReadStatusAsync(gameId, userId, cancellationToken);
        var lastReadTime = lastReadTimestamp ?? DateTime.UtcNow;

        var readMessages = new List<GameChatMessageDto>();
        var unreadMessages = new List<GameChatMessageDto>();

        foreach (var message in messages)
        {
            var userInfo = users.GetValueOrDefault(message.UserId);
            var messageDto = new GameChatMessageDto
            {
                MessageId = message.MessageId,
                GameId = message.GameId,
                UserId = message.UserId,
                UserName = userInfo?.FullName ?? "Unknown",
                Avartar = userInfo?.Avartar ?? "default-avatar.png",
                Message = message.Message,
                CreatedOn = message.CreatedOn
            };
            if (message.CreatedOn > lastReadTime)
            {
                unreadMessages.Add(messageDto);
            }
            else
            {
                readMessages.Add(messageDto);
            }
        }

        var pageInfo = new PageInfo
        {
            HasNextPage = messages.Count == request.Limit,
            StartCursor = messages.FirstOrDefault()?.CreatedOn.ToString("o"),
            EndCursor = messages.LastOrDefault()?.CreatedOn.ToString("o")
        };

        var data = new PagedMessagesResponse<GameChatMessageDto>
        {
            PageInfo = pageInfo,
            UnreadMessages = unreadMessages,
            ReadMessages = readMessages
        };

        return res.SetSuccess(data);
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// GameChat Service
    /// </summary>
    private readonly IGameChatService _gameChatService;

    /// <summary>
    /// User Cached Service
    /// </summary>
    private readonly IUserCacheService _userCacheService;

    #endregion
}

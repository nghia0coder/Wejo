using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Queries;

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
public class UserChatGetMessageH : BaseH, IRequestHandler<UserChatGetMessageR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public UserChatGetMessageH(IWejoContext context, IUserChatService UserChatService, IUserCacheService userCacheService) : base(context)
    {
        _userChatService = UserChatService;
        _userCacheService = userCacheService;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserChatGetMessageR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        #region -- Validate on client --
        var vr = new UserChatGetMessageV().Validate(request);
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
        //var hasPlaypal = await _context.UserPlaypals.AnyAsync(p => p.UserId1 == request.ReceiverId && p.UserId2 == request.UserId ||
        //                                                              p.UserId2 == request.ReceiverId && p.UserId1 == request.UserId, cancellationToken);
        //if (!hasPlaypal)
        //{
        //    var t = new List<DicDto> { new() { Key = nameof(request.Id).ToCamelCase(), Value = request.Id } };
        //    return res.SetErrorData(nameof(E206), E206, null);
        //}
        #endregion

        var conversationId = request.Id;

        var messages = await _userChatService.GetMessagesAsync(conversationId, request.Before, request.After, userId,
            request.Limit, cancellationToken);

        var (lastReadMessageId, lastReadTimestamp) = await _userChatService.GetReadStatusAsync(conversationId, userId, cancellationToken);
        var lastReadTime = lastReadTimestamp ?? DateTime.UtcNow;
        var userIds = messages.Select(m => m.SenderId).Distinct().ToList();
        var users = new Dictionary<string, UserInfoDto>();

        foreach (var uid in userIds)
        {
            var userInfo = await _userCacheService.GetUserInfoAsync(uid, cancellationToken);
            users[uid] = userInfo;
        }

        var readMessages = new List<UserChatMessageDto>();
        var unreadMessages = new List<UserChatMessageDto>();

        foreach (var message in messages)
        {
            var userInfo = users.GetValueOrDefault(message.SenderId);
            var messageDto = new UserChatMessageDto
            {
                MessageId = message.MessageId,
                SenderId = message.SenderId,
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

        var data = new PagedMessagesResponse<UserChatMessageDto>
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
    /// UserChat Service
    /// </summary>
    private readonly IUserChatService _userChatService;

    /// <summary>
    /// User Cached Service
    /// </summary>
    private readonly IUserCacheService _userCacheService;

    #endregion
}

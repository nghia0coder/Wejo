using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Commands;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Interfaces;
using Requests;
using Validators;
using Wejo.Common.Core.Constants;
using Wejo.Identity.Infrastructure.MessageQueue;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserChatSendMessageH : BaseH, IRequestHandler<UserChatSendMessageR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public UserChatSendMessageH(IWejoContext context, IUserChatService userChatService, IMessageQueue messageQueue) : base(context)
    {
        _userChatService = userChatService;
        _messageQueue = messageQueue;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserChatSendMessageR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        #region -- Validate on client --
        var vr = new UserChatSendMessageV().Validate(request);
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
        var reveiverId = request.ReceiverId;
        if (reveiverId == null)
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
        var hasPlaypal = await _context.UserPlaypals.AnyAsync(p => p.UserId1 == request.ReceiverId && p.UserId2 == request.UserId ||
                                                                      p.UserId2 == request.ReceiverId && p.UserId1 == request.UserId, cancellationToken);
        if (!hasPlaypal)
        {
            var t = new List<DicDto> { new() { Key = nameof(request.Id).ToCamelCase(), Value = request.Id } };
            return res.SetErrorData(nameof(E206), E206, null);
        }
        #endregion

        var conversationId = await _userChatService.GetConversationAsync(userId, reveiverId, cancellationToken);

        var data = await _userChatService.SendMessageAsync(conversationId, request, cancellationToken);

        await _messageQueue.PublishAsync(QueueName.PlaypalChatMessage, new { Id = conversationId.ToString(), Message = data });

        return res.SetSuccess(data);
    }

    #endregion

    #region -- Fields --

    private readonly IUserChatService _userChatService;

    /// <summary>
    /// Message queue
    /// </summary>
    private readonly IMessageQueue _messageQueue;

    #endregion
}

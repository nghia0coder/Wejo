using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Commands;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Infrastructure.MessageQueue;
using Interfaces;
using Request;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserChatMarkAsReadH : BaseH, IRequestHandler<UserChatMarkAsReadR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public UserChatMarkAsReadH(IWejoContext context, IUserChatService UserChatService, IMessageQueue messageQueue) : base(context)
    {
        _userChatService = UserChatService;
        _messageQueue = messageQueue;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserChatMarkAsReadR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        #region -- Validate on client --
        var vr = new UserChatMarkAsReadV().Validate(request);
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
        //var hasParticipant = await _context.UserParticipants.AnyAsync(p => p.UserId == request.Id &&
        //                                                              p.UserId == request.UserId &&
        //                                                              p.Status == PlayerStatus.Accepted, cancellationToken);
        //if (!hasParticipant)
        //{
        //    var t = new List<DicDto> { new() { Key = nameof(request.Id).ToCamelCase(), Value = request.Id } };
        //    return res.SetErrorData(nameof(E205), E205, t);
        //}
        #endregion


        await _userChatService.UpdateReadStatusAsync(request.Id, userId, request.LastReadMessageId, request.LastReadTimestamp, cancellationToken);

        return res.SetSuccess(200);
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// UserChat Service
    /// </summary>
    private readonly IUserChatService _userChatService;

    /// <summary>
    /// Message queue
    /// </summary>
    private readonly IMessageQueue _messageQueue;

    #endregion
}

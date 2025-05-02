using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Commands;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Request;
using Validators;
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
    public UserChatSendMessageH(IWejoContext context) : base(context)
    {
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
            return res.SetErrorData(nameof(E206), E206, t);
        }
        #endregion

        //await _gameChatService.UpdateReadStatusAsync(request.Id, userId, request.LastReadMessageId, request.LastReadTimestamp, cancellationToken);

        return res.SetSuccess(200);
    }

    #endregion

    #region -- Fields --

    #endregion
}

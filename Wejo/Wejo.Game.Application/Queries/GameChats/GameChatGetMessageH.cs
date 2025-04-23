using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Queries;

using Common.Core.Enums;
using Common.Core.Extensions;
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
    public GameChatGetMessageH(IWejoContext context, IGameChatService gameChatService) : base(context)
    {
        _gameChatService = gameChatService;
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

        var data = await _gameChatService.GetMessagesAsync(request.Id, request.Before, request.After, request.FromUser,
            request.Limit, cancellationToken);

        return res.SetSuccess(data);
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// GameChat Service
    /// </summary>
    private readonly IGameChatService _gameChatService;

    #endregion
}

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Commands;

using Common.Core.Constants;
using Common.Core.Enums;
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
public class GameChatSendMessageH : BaseH, IRequestHandler<GameChatSendMessageR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameChatSendMessageH(IWejoContext context, IGameChatService gameChatService, IMessageQueue messageQueue) : base(context)
    {
        _gameChatService = gameChatService;
        _messageQueue = messageQueue;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(GameChatSendMessageR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        #region -- Validate on client --
        var vr = new GameChatSendMessageV().Validate(request);
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


        var data = await _gameChatService.SendMessageAsync(request.Id, userId, request, cancellationToken);

        await _messageQueue.PublishAsync(QueueName.GameChatMessage, new { GameId = request.Id.ToString(), Message = data });

        return res.SetSuccess(data);
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// GameChat Service
    /// </summary>
    private readonly IGameChatService _gameChatService;

    /// <summary>
    /// Message queue
    /// </summary>
    private readonly IMessageQueue _messageQueue;

    #endregion
}

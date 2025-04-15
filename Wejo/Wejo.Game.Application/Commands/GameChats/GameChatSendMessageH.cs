using Cassandra;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Commands;

using Common.Core.Enums;
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
public class GameChatSendMessageH : BaseH, IRequestHandler<GameChatSendMessageR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameChatSendMessageH(IWejoContext context, ISession cassandraSession) : base(context)
    {
        _cassandraSession = cassandraSession;
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

        var gameId = request.Id;

        // Tạo tin nhắn
        var messageId = Guid.NewGuid();
        var createdOn = DateTime.UtcNow;
        var bucket = int.Parse(createdOn.ToString("yyyyMM")); // Bucket theo tháng (202504)

        // Lưu vào Cassandra
        var insertQuery = "INSERT INTO game_chat_messages (game_id, bucket, message_id, user_id, message, created_on) VALUES (?, ?, ?, ?, ?, ?) USING TTL 604800";
        var preparedStatement = await _cassandraSession.PrepareAsync(insertQuery);
        var boundStatement = preparedStatement.Bind(gameId, bucket, messageId, userId, request.Message, createdOn);
        await _cassandraSession.ExecuteAsync(boundStatement);

        // Lấy thông tin người gửi
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
            .FirstAsync(cancellationToken);

        var messageDto = new
        {
            Id = messageId,
            GameId = gameId,
            UserId = userId,
            UserName = user.FullName,
            request.Message,
            CreatedOn = createdOn
        };


        return res.SetSuccess(messageDto);
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// Cassandra Session
    /// </summary>
    private readonly ISession _cassandraSession;

    #endregion
}

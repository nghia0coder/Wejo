using Cassandra;
using CassandraQueryBuilder;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Services;

using Commands;
using Common.Domain.Dtos;
using Common.Domain.Interfaces;
using Domain.Entities;
using Interfaces;
using Request;

/// <summary>
/// Implementation
/// </summary>
public class GameChatService : BaseH, IGameChatService
{
    #region -- Fields --

    private readonly ISession _cassandraSession;

    private readonly PreparedStatement _insertMessageStatement;

    private const string TableName = "game_chat_messages";

    #endregion

    #region -- Constructor --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameChatService(IWejoContext context, ISession cassandraSession) : base(context)
    {
        _cassandraSession = cassandraSession;

        var insertQuery = new Insert().Keyspace("wejo")
            .Table(TableName)
            .TTL()
            .InsertColumns(
                GameChatMessage.GAME_ID,
                GameChatMessage.BUCKET,
                GameChatMessage.MESSAGE_ID,
                GameChatMessage.USER_ID,
                GameChatMessage.MESSAGE,
                GameChatMessage.CREATED_ON
            )
            .ToString();

        _insertMessageStatement = _cassandraSession.Prepare(insertQuery);
    }

    #endregion

    #region -- Methods --

    /// <summary>
    /// Handle
    /// </summary>
    public async Task<GameChatMessageDto> SendMessageAsync(Guid gameId, string userId, GameChatSendMessageR request, CancellationToken cancellationToken)
    {
        var messageId = Guid.NewGuid();
        var createdOn = DateTime.UtcNow;
        var bucket = int.Parse(createdOn.ToString("yyyyMM")); // E.g., 202504

        var boundStatement = _insertMessageStatement.Bind(
            gameId,
            bucket,
            messageId,
            userId,
            request.Message,
            createdOn
        );

        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
            .FirstAsync(cancellationToken);

        var messageDto = new GameChatMessageDto
        {
            Id = messageId,
            GameId = gameId,
            UserId = user.Id,
            UserName = user.FullName,
            Message = request.Message,
            CreatedOn = createdOn
        };

        await _cassandraSession.ExecuteAsync(boundStatement);

        return messageDto;
    }

    #endregion
}

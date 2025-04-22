using Cassandra;
using CassandraQueryBuilder;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Services;

using Commands;
using Common.Core.Extensions;
using Common.Domain.Dtos;
using Common.Domain.Interfaces;
using Common.SeedWork;
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

    private readonly ICassandraStatementFactory _statementFactory;

    private readonly ChatConfig _config;

    private const string TableName = "game_chat_messages";

    #endregion

    #region -- Constructor --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameChatService(IWejoContext context, ISession cassandraSession, ChatConfig config,
                           ICassandraStatementFactory cassandraStatementFactory) : base(context)
    {
        _cassandraSession = cassandraSession;
        _statementFactory = cassandraStatementFactory;
        _config = config;

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

    /// <inheritdoc/>
    public async Task<List<GameChatMessage>> GetMessagesAsync(
        Guid gameId,
        DateTime? before,
        DateTime? after,
        string? fromUserId,
        int limit,
        CancellationToken cancellationToken)
    {
        var messages = new List<GameChatMessage>();
        var endDate = before ?? DateTime.UtcNow;
        var startDate = after ?? endDate.AddMonths(-_config.DefaultHistoryMonths);
        var buckets = DateTimeExtension.GenerateBuckets(startDate, endDate);

        foreach (var bucket in buckets)
        {
            var statement = GetAppropriateStatement(before, after, fromUserId);
            var bindParams = CreateBindParameters(gameId, bucket, before, after, fromUserId, limit);
            var boundStatement = statement.Bind(bindParams);

            var rows = await _cassandraSession.ExecuteAsync(boundStatement);
            messages.AddRange(MapRowsToMessages(rows, gameId));

            if (messages.Count >= limit)
                break;
        }

        return messages.Count <= limit ? messages : messages.GetRange(0, limit);
    }

    /// <inheritdoc/>
    public async Task<Dictionary<string, (Guid? LastReadMessageId, DateTime? LastReadTimestamp)>> GetReadStatusAsync(
        Guid gameId,
        List<string> userIds,
        CancellationToken cancellationToken)
    {
        var result = InitializeReadStatusResult(userIds);

        var boundStatement = _statementFactory.CreateSelectReadStatusStatement().Bind(gameId, userIds);
        var rows = await _cassandraSession.ExecuteAsync(boundStatement).ConfigureAwait(false);

        foreach (var row in rows)
        {
            var userId = row.GetValue<string>("user_id");
            var lastReadMessageId = row.GetValue<Guid?>("last_read_message_id");
            var lastReadTimestamp = row.GetValue<DateTime?>("last_read_timestamp");
            result[userId] = (lastReadMessageId, lastReadTimestamp);
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task UpdateReadStatusAsync(
        Guid gameId,
        string userId,
        Guid lastReadMessageId,
        DateTime lastReadTimestamp,
        CancellationToken cancellationToken)
    {
        var boundStatement = _statementFactory.CreateUpdateReadStatusStatement().Bind(
            gameId,
            userId,
            lastReadMessageId,
            lastReadTimestamp
        );

        await _cassandraSession.ExecuteAsync(boundStatement).ConfigureAwait(false);
    }

    #endregion

    #region Private Helper Methods

    private PreparedStatement GetAppropriateStatement(DateTime? before, DateTime? after, string? fromUserId)
    {
        if (fromUserId != null)
        {
            if (before.HasValue)
                return _statementFactory.CreateSelectMessagesBeforeFromUserStatement();
            else if (after.HasValue)
                return _statementFactory.CreateSelectMessagesAfterFromUserStatement();
            else
                return _statementFactory.CreateSelectMessagesBeforeFromUserStatement();
        }
        else
        {
            if (before.HasValue)
                return _statementFactory.CreateSelectMessagesBeforeStatement();
            else if (after.HasValue)
                return _statementFactory.CreateSelectMessagesAfterStatement();
            else
                return _statementFactory.CreateSelectMessagesBeforeStatement();
        }
    }
    private object[] CreateBindParameters(
        Guid gameId,
        int bucket,
        DateTime? before,
        DateTime? after,
        string? fromUserId,
        int limit)
    {
        if (fromUserId != null)
        {
            if (before.HasValue)
                return new object[] { gameId, bucket, before.Value, fromUserId, limit };
            else if (after.HasValue)
                return new object[] { gameId, bucket, after.Value, fromUserId, limit };
            else
                return new object[] { gameId, bucket, DateTime.UtcNow, fromUserId, limit };
        }
        else
        {
            if (before.HasValue)
                return new object[] { gameId, bucket, before.Value, limit };
            else if (after.HasValue)
                return new object[] { gameId, bucket, after.Value, limit };
            else
                return new object[] { gameId, bucket, DateTime.UtcNow, limit };
        }
    }

    private IEnumerable<GameChatMessage> MapRowsToMessages(RowSet rows, Guid gameId)
    {
        foreach (var row in rows)
        {
            yield return new GameChatMessage
            {
                MessageId = row.GetValue<Guid>("message_id"),
                GameId = gameId,
                UserId = row.GetValue<string>("user_id"),
                Message = row.GetValue<string>("message"),
                CreatedOn = row.GetValue<DateTime>("created_on")
            };
        }
    }

    private Dictionary<string, (Guid? LastReadMessageId, DateTime? LastReadTimestamp)> InitializeReadStatusResult(List<string> userIds)
    {
        var result = new Dictionary<string, (Guid? LastReadMessageId, DateTime? LastReadTimestamp)>();
        foreach (var userId in userIds)
        {
            result[userId] = (null, null);
        }
        return result;
    }

    #endregion
}

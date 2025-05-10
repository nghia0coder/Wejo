using Cassandra;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Services;

using Commands;
using Common.Core.Extensions;
using Common.Domain.Dtos;
using Common.Domain.Interfaces;
using Common.SeedWork;
using Interfaces;
using Requests;

/// <summary>
/// Implementation
/// </summary>
public class UserChatService : BaseH, IUserChatService
{
    #region -- Fields --

    private readonly ISession _cassandraSession;

    private readonly ICassandraStatementFactory _statementFactory;

    private readonly ChatConfig _config;

    #endregion

    #region -- Constructor --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public UserChatService(IWejoContext context, ISession cassandraSession, ChatConfig config,
                           ICassandraStatementFactory cassandraStatementFactory) : base(context)
    {
        _cassandraSession = cassandraSession;
        _statementFactory = cassandraStatementFactory;
        _config = config;
    }

    #endregion

    #region -- Methods --

    /// <summary>
    /// Handle
    /// </summary>
    public async Task<UserChatMessageDto> SendMessageAsync(Guid conversationId, UserChatSendMessageR request, CancellationToken cancellationToken)
    {
        var messageId = Guid.NewGuid();
        var createdOn = DateTime.UtcNow;

        var batch = new BatchStatement();

        var insertMessageStatement = _statementFactory.CreateInsertMessageStatement();
        var boundStatement = insertMessageStatement.Bind(
            conversationId,
            createdOn,
            messageId,
            request.Message,
            request.UserId
        );
        batch.Add(boundStatement);
        var user = await _context.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
            .FirstAsync(cancellationToken);

        await _cassandraSession.ExecuteAsync(batch).ConfigureAwait(false);

        var messageDto = new UserChatMessageDto
        {
            ConversationId = conversationId,
            CreatedOn = createdOn,
            MessageId = messageId,
            Message = request.Message,
            SenderId = user.Id,
            UserName = user.FullName,
        };

        return messageDto;
    }

    public async Task<Guid> GetConversationAsync(string user1, string user2, CancellationToken cancellationToken)
    {
        var (userId1, userId2) = SortUserIds(user1, user2);

        var boundStatement = _statementFactory.CreateSelectConversationStatement().Bind(userId1, userId2);

        var row = _cassandraSession.ExecuteAsync(boundStatement).ConfigureAwait(false).GetAwaiter().GetResult().FirstOrDefault();

        if (row == null)
        {
            // Không tìm thấy cuộc trò chuyện, tạo mới
            return await CreateConversationAsync(user1, user2, cancellationToken);
        }

        var conversationId = row.GetValue<Guid>("conversation_id");
        return conversationId;
    }

    public async Task<Guid> CreateConversationAsync(string userId1, string userId2, CancellationToken cancellationToken)
    {
        var (userId1Sorted, userId2Sorted) = SortUserIds(userId1, userId2);

        // Tạo conversation_id mới
        var conversationId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var conversation = new Conversation
        {
            Id = conversationId,
            UserId1 = userId1Sorted,
            UserId2 = userId2Sorted,
            CreatedAt = createdAt,
            DeletedByUser1 = false,
            DeletedByUser2 = false
        };

        // Lưu vào playpal_conversations
        var conversationStatement = _statementFactory.CreateInsertConversationStatement().Bind(
            conversationId,
            userId1Sorted,
            userId2Sorted,
            createdAt,
            false,
            false);
        await _cassandraSession.ExecuteAsync(conversationStatement).ConfigureAwait(false);

        // Lưu vào playpal_conversations_by_users
        var conversationByUsersStatement = _statementFactory.CreateInsertConversationByUserStatement().Bind(
            userId1Sorted,
            userId2Sorted,
            conversationId,
            createdAt);
        await _cassandraSession.ExecuteAsync(conversationByUsersStatement).ConfigureAwait(false);

        return conversationId;
    }

    /// <inheritdoc/>
    public async Task<List<UserChatMessageDto>> GetMessagesAsync(
        Guid UserId,
        DateTime? before,
        DateTime? after,
        string? fromUserId,
        int limit,
        CancellationToken cancellationToken)
    {
        var messages = new List<UserChatMessageDto>();
        var endDate = before ?? DateTime.UtcNow;
        var startDate = after ?? endDate.AddMonths(-_config.DefaultHistoryMonths);
        var buckets = DateTimeExtension.GenerateBuckets(startDate, endDate);

        foreach (var bucket in buckets)
        {
            var statement = GetAppropriateStatement(before, after, fromUserId);
            var bindParams = CreateBindParameters(UserId, bucket, before, after, fromUserId, limit);
            var boundStatement = statement.Bind(bindParams);

            var rows = await _cassandraSession.ExecuteAsync(boundStatement);
            messages.AddRange(MapRowsToMessages(rows, UserId));

            if (messages.Count >= limit)
                break;
        }

        return messages.Count <= limit ? messages : messages.GetRange(0, limit);
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
                return _statementFactory.CreateSelectMessagesBeforeStatement();
            else
                return _statementFactory.CreateSelectMessagesBeforeStatement();
        }
    }
    private object[] CreateBindParameters(
        Guid UserId,
        int bucket,
        DateTime? before,
        DateTime? after,
        string? fromUserId,
        int limit)
    {
        if (fromUserId != null)
        {
            if (before.HasValue)
                return new object[] { UserId, bucket, before.Value, fromUserId, limit };
            else if (after.HasValue)
                return new object[] { UserId, bucket, after.Value, fromUserId, limit };
            else
                return new object[] { UserId, bucket, DateTime.UtcNow, fromUserId, limit };
        }
        else
        {
            if (before.HasValue)
                return new object[] { UserId, bucket, before.Value, limit };
            else if (after.HasValue)
                return new object[] { UserId, bucket, after.Value, limit };
            else
                return new object[] { UserId, bucket, DateTime.UtcNow, limit };
        }
    }

    private (string UserId1Sorted, string UserId2Sorted) SortUserIds(string userId1, string userId2)
    {
        var sortedUserIds = new[] { userId1, userId2 }.OrderBy(id => id).ToArray();
        return (sortedUserIds[0], sortedUserIds[1]);
    }

    private IEnumerable<UserChatMessageDto> MapRowsToMessages(RowSet rows, Guid UserId)
    {
        foreach (var row in rows)
        {
            yield return new UserChatMessageDto
            {
                MessageId = row.GetValue<Guid>("message_id"),
                SenderId = row.GetValue<string>("sender_idc   "),
                Message = row.GetValue<string>("message"),
                CreatedOn = row.GetValue<DateTime>("created_on")
            };
        }
    }

    private Conversation MapRowToConversation(Row row)
    {
        return new Conversation
        {
            Id = row.GetValue<Guid>("conversation_id"),
            UserId1 = row.GetValue<string>("user_id_1"),
            UserId2 = row.GetValue<string>("user_id_2"),
            CreatedAt = row.GetValue<DateTime>("created_at"),
            DeletedByUser1 = row.GetValue<bool>("deleted_by_user_1"),
            DeletedByUser2 = row.GetValue<bool>("deleted_by_user_2")
        };
    }

    #endregion
}

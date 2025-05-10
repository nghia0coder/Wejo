using Cassandra;
using CassandraQueryBuilder;

namespace Wejo.Identity.Application.Services;

using Common.Domain.Entities;
using Common.SeedWork;
using Interfaces;

/// <summary>
/// Implementation of the Cassandra statement factory
/// </summary>
public class CassandraStatementFactory : ICassandraStatementFactory
{
    private readonly ISession _cassandraSession;
    private readonly ChatConfig _config;

    private readonly string[] TableName = { "playpal_conversations", "playpal_conversations_by_users", "playpal_messages" };
    private static readonly Column[] MessageColumns = [UserPlaypalMessage.CONVERSATION_ID,
                                                       UserPlaypalMessage.CREATED_ON,
                                                       UserPlaypalMessage.MESSAGE_ID,
                                                       UserPlaypalMessage.MESSAGE,
                                                       UserPlaypalMessage.SENDER_ID];

    /// <summary>
    /// Initializes a new instance of the CassandraStatementFactory
    /// </summary>
    /// <param name="cassandraSession">Cassandra session</param>
    /// <param name="config">Repository configuration</param>
    public CassandraStatementFactory(ISession cassandraSession, ChatConfig config)
    {
        _cassandraSession = cassandraSession ?? throw new ArgumentNullException(nameof(cassandraSession));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    /// <inheritdoc/>
    public PreparedStatement CreateInsertMessageStatement()
    {
        var insertQuery = new Insert().Keyspace("wejo")
            .Table(TableName[2])
            .TTL()
            .InsertColumns(MessageColumns)
            .ToString();
        return _cassandraSession.Prepare(insertQuery);
    }

    /// <inheritdoc/>
    public PreparedStatement CreateInsertMessageByUserStatement()
    {
        var insertQuery = new Insert().Keyspace("wejo")
            .Table(TableName[1])
            .TTL()
            .InsertColumns(MessageColumns)
            .ToString();
        return _cassandraSession.Prepare(insertQuery);
    }

    /// <inheritdoc/>
    public PreparedStatement CreateSelectMessagesBeforeStatement()
    {
        return _cassandraSession.Prepare(
            $"SELECT message_id, user_id, message, created_on " +
            $"FROM {TableName[0]} WHERE game_id = ? AND bucket = ? AND created_on < ? LIMIT ?");
    }

    /// <inheritdoc/>
    public PreparedStatement CreateSelectMessagesAfterStatement()
    {
        return _cassandraSession.Prepare(
            $"SELECT message_id, user_id, message, created_on " +
            $"FROM {TableName[0]} WHERE game_id = ? AND bucket = ? AND created_on > ? LIMIT ?");
    }

    /// <inheritdoc/>
    public PreparedStatement CreateSelectMessagesBeforeFromUserStatement()
    {
        return _cassandraSession.Prepare(
            $"SELECT message_id, user_id, message, created_on " +
            $"FROM {TableName[1]} WHERE game_id = ? AND bucket = ? AND created_on < ? AND user_id = ? LIMIT ?");
    }

    /// <inheritdoc/>
    public PreparedStatement CreateSelectMessagesAfterFromUserStatement()
    {
        return _cassandraSession.Prepare(
            $"SELECT message_id, user_id, message, created_on " +
            $"FROM {TableName[1]} WHERE game_id = ? AND bucket = ? AND created_on > ? AND user_id = ? LIMIT ?");
    }

    /// <inheritdoc/>
    public PreparedStatement CreateInsertConversationStatement()
    {
        return _cassandraSession.Prepare(
            @"INSERT INTO playpal_conversations (conversation_id, user_id_1, user_id_2, created_at, deleted_by_user_1, deleted_by_user_2)
              VALUES (?, ?, ?, ?, ?, ?)");
    }

    /// <inheritdoc/>
    public PreparedStatement CreateInsertConversationByUserStatement()
    {
        return _cassandraSession.Prepare(
            @"INSERT INTO playpal_conversations_by_users (user_id_1, user_id_2, conversation_id, created_at)
              VALUES (?, ?, ?, ?)");
    }

    /// <inheritdoc/>
    public PreparedStatement CreateSelectConversationStatement()
    {
        return _cassandraSession.Prepare(
            $"SELECT conversation_id, created_at FROM {TableName[1]} WHERE user_id_1 = ? AND user_id_2 = ?");
    }
}

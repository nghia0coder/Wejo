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

    private readonly string[] TableName = { "game_chat_messages", "game_chat_messages_by_user" };
    private const string ReadStatusTableName = "game_chat_read_status";
    private static readonly Column[] MessageColumns = [UserPlaypalMessage.CONVERSATION_ID,
                                                       UserPlaypalMessage.MESSAGE_ID,
                                                       UserPlaypalMessage.USER_ID,
                                                       UserPlaypalMessage.MESSAGE,
                                                       UserPlaypalMessage.CREATED_ON];

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
            .Table(TableName[0])
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
    public PreparedStatement CreateSelectReadStatusStatement()
    {
        return _cassandraSession.Prepare(
            $"SELECT user_id, last_read_message_id, last_read_timestamp FROM {ReadStatusTableName} WHERE game_id = ? AND user_id IN ?");
    }

    /// <inheritdoc/>
    public PreparedStatement CreateUpdateReadStatusStatement()
    {
        return _cassandraSession.Prepare(
            $"INSERT INTO {ReadStatusTableName} (game_id, user_id, last_read_message_id, last_read_timestamp) VALUES (?, ?, ?, ?)");
    }
}

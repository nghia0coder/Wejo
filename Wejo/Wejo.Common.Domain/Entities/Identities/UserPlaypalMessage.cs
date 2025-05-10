using CassandraQueryBuilder;

namespace Wejo.Common.Domain.Entities;

public class UserPlaypalMessage
{
    public static readonly Column CONVERSATION_ID = new Column("conversation_id", ColumnType.UUID);

    public static readonly Column CREATED_ON = new Column("created_on", ColumnType.TIMESTAMP);

    public static readonly Column MESSAGE_ID = new Column("message_id", ColumnType.UUID);

    public static readonly Column MESSAGE = new Column("message", ColumnType.TEXT);

    public static readonly Column SENDER_ID = new Column("sender_id", ColumnType.TEXT);
}

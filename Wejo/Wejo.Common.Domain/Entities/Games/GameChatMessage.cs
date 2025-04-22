using CassandraQueryBuilder;

namespace Wejo.Game.Domain.Entities;

public class GameChatMessage
{
    #region -- Properties --

    public Guid MessageId { get; set; }
    public Guid GameId { get; set; }
    public string? UserId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedOn { get; set; }

    #endregion

    #region  -- Columns --

    public static readonly Column MESSAGE_ID = new Column("message_id", ColumnType.UUID);

    public static readonly Column GAME_ID = new Column("game_id", ColumnType.UUID);

    public static readonly Column BUCKET = new Column("bucket", ColumnType.INT);

    public static readonly Column USER_ID = new Column("user_id", ColumnType.TEXT);

    public static readonly Column MESSAGE = new Column("message", ColumnType.TEXT);

    public static readonly Column CREATED_ON = new Column("created_on", ColumnType.TIMESTAMP);

    #endregion
}

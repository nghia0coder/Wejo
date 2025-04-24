namespace Wejo.Game.Application.Request;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class GameChatMarkAsReadR : IdBaseR
{
    /// <summary>
    /// Last message read
    /// </summary>
    public Guid LastReadMessageId { get; set; }

    /// <summary>
    /// Timestamp of the last read message
    /// </summary>
    public DateTime LastReadTimestamp { get; set; }
}

namespace Wejo.Identity.Application.Request;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class UserChatMarkAsReadR : IdBaseR
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

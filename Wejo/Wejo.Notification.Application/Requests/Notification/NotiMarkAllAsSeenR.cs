namespace Wejo.Notification.Application.Requests;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class NotiMarkAllAsSeenR : BaseR
{
    /// <summary>
    /// Notification type
    /// </summary>
    public string? Type { get; set; }
}

using Wejo.Common.Core.Requests;

namespace Wejo.Notification.Application.Requests;

/// <summary>
/// Request
/// </summary>
public class NotificationViewR : BaseR
{
    /// <summary>
    /// Notification type
    /// </summary>
    public string? Type { get; set; }
}

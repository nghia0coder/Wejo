namespace Wejo.Common.Domain.Entities;

/// <summary>
/// Stores user preferences for notification types
/// </summary>
public partial class NotificationSetting
{
    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Type of notification (enum)
    /// </summary>
    public int NotificationType { get; set; }

    /// <summary>
    /// Whether this notification type is enabled for the user
    /// </summary>
    public bool IsEnabled { get; set; }
}

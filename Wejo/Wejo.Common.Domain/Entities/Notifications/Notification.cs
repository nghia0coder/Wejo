namespace Wejo.Common.Domain.Entities;

using Common.SeedWork;
using Core.Enums;

/// <summary>
/// Stores user notifications
/// </summary>
public partial class Notification : EntityId
{
    /// <summary>
    /// Unique identifier for the notification
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Recipient user ID
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Notification type (enum: 1=GameStarted, 2=GameEnded, 3=PlaypalAdded, etc.)
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Short title of the notification
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Detailed message content
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Optional ID of related entity (e.g., GameId)
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// Indicates if the notification has been read
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Indicates if the notification has been seen
    /// </summary>
    public bool IsSeen { get; set; }

    /// <summary>
    /// Timestamp when the notification was created
    /// </summary>
    public DateTime CreatedOn { get; set; }
}

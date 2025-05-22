namespace Wejo.Notification.Application.Filters;

using Common.Core.Enums;
using Common.Core.Filters;

/// <summary>
/// Filter
/// </summary>
public class NotificationFilter : BaseFilter
{
    #region -- Classes --

    /// <summary>
    /// Search
    /// </summary>
    public new class Search : BaseFilter.Search
    {
        /// <summary>
        /// Notification Type
        /// </summary>
        public NotificationType Type { get; set; }
    }

    #endregion
}

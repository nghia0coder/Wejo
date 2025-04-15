namespace Wejo.Common.Core.Extentions;

using Enums;

/// <summary>
/// Provides predefined notification templates, including titles and message formats,  
/// for different types of notifications.
/// </summary>
public static class MessageExtension
{
    #region -- Methods --

    /// <summary>
    /// A dictionary containing notification templates for each <see cref="NotificationType"/>.  
    /// Each template includes a title and a message format string.
    /// </summary>
    public static readonly Dictionary<NotificationType, (string Title, string MessageTemplate)> Templates = new()
    {
        { NotificationType.Game, ("New Join Request", "{0} has requested to join your {1} game happening on {2}.") }
    };

    #endregion
}

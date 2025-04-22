namespace Wejo.Common.SeedWork;

public class ChatConfig
{
    /// <summary>
    /// Time to live for messages in seconds
    /// </summary>
    public int MessageTtlSeconds { get; set; } = 604800; // 7 days

    /// <summary>
    /// Default number of months to look back for history
    /// </summary>
    public int DefaultHistoryMonths { get; set; } = 1;

}

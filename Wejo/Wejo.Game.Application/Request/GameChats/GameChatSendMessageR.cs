namespace Wejo.Game.Application.Request;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class GameChatSendMessageR : IdBaseR
{
    /// <summary>
    /// User message
    /// </summary>
    public string Message { get; set; } = null!;
}

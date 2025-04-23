namespace Wejo.Game.Application.Request;

using Wejo.Common.Core.Requests;

public class GameChatGetMessageR : IdBaseR
{
    public DateTime? Before { get; set; }
    public DateTime? After { get; set; }
    public string? FromUser { get; set; }
    public int Limit { get; set; } = 20;
}

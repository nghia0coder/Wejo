namespace Wejo.Game.Application.Request;

public class GameChatGetMessageR
{
    public Guid GameId { get; set; }
    public DateTime Before { get; set; }
    public DateTime After { get; set; }
    public string? FromUserId { get; set; }
    public int Limit { get; set; } = 20;
}

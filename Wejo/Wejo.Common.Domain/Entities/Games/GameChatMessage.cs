namespace Wejo.Game.Domain.Entities;

public class GameChatMessage
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public int Bucket { get; set; }
    public string UserId { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
}
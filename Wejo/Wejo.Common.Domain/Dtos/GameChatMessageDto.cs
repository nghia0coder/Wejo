namespace Wejo.Common.Domain.Dtos;

public class GameChatMessageDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
}

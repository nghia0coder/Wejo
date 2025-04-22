namespace Wejo.Common.Domain.Dtos;

using SeedWork.Dtos;

public class GameChatMessageDto : MessageDto
{
    public Guid GameId { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
}

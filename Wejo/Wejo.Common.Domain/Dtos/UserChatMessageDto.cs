namespace Wejo.Common.Domain.Dtos;

using SeedWork.Dtos;

public class UserChatMessageDto : MessageDto
{
    public Guid ConversationId { get; set; }
    public string SenderId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Avartar { get; set; } = null!;
}

public class Conversation
{
    public Guid Id { get; set; }
    public string? UserId1 { get; set; }
    public string? UserId2 { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool DeletedByUser1 { get; set; }
    public bool DeletedByUser2 { get; set; }
}

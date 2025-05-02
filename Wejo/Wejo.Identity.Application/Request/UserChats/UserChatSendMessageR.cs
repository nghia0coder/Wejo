namespace Wejo.Identity.Application.Request;

using Common.Core.Requests;
public class UserChatSendMessageR : IdBaseR
{
    public Guid? ConversationId { get; set; }

    public string? ReceiverId { get; set; }

    public string? Message { get; set; }
}

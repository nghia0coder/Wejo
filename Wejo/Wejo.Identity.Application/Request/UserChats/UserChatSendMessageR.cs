namespace Wejo.Identity.Application.Requests;

using Common.Core.Requests;
public class UserChatSendMessageR : IdBaseR
{
    public string? ReceiverId { get; set; }

    public string? Message { get; set; }
}

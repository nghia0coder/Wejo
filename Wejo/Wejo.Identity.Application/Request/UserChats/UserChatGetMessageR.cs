namespace Wejo.Identity.Application.Request;

using Common.Core.Requests;

public class UserChatGetMessageR : IdBaseR
{
    public DateTime? Before { get; set; }
    public DateTime? After { get; set; }
    public string? FromUser { get; set; }
    public int Limit { get; set; } = 20;
}

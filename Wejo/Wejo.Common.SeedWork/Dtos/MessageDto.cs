namespace Wejo.Common.SeedWork.Dtos;

public abstract class MessageDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public Dictionary<string, string> SeenBy { get; set; } = new Dictionary<string, string>();
}


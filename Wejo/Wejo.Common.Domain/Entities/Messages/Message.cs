namespace Wejo.Common.Domain.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid GameId { get; set; }

    public string UserId { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public string Content { get; set; } = null!;

    public bool IsComment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Message> InverseParent { get; set; } = new List<Message>();

    public virtual Message? Parent { get; set; }
}

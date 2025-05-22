namespace Wejo.Common.Domain.Entities;

public partial class ParticipantHistory
{
    public Guid Id { get; set; }

    public int? Action { get; set; }

    public DateOnly? TimeStamp { get; set; }

    public string? Details { get; set; }

    public Guid ParticipantId { get; set; }

    public virtual GameParticipant Participant { get; set; } = null!;
}

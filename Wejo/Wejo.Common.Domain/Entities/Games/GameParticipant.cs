namespace Wejo.Common.Domain.Entities;

public partial class GameParticipant
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public Guid GameId { get; set; }

    public short Status { get; set; }

    public DateTime JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual ICollection<ParticipantHistory> ParticipantHistories { get; set; } = new List<ParticipantHistory>();

    public virtual User User { get; set; } = null!;
}

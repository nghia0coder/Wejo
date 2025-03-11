namespace Wejo.Common.Domain.Entities;

using Common.SeedWork;
using Core.Enums;

public partial class GameParticipant : EntityId
{
    public string UserId { get; set; } = null!;

    public Guid GameId { get; set; }

    public PlayerStatus Status { get; set; }

    public DateTime JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual ICollection<ParticipantHistory> ParticipantHistories { get; set; } = new List<ParticipantHistory>();

    public virtual User User { get; set; } = null!;

    public DateTime? ModifiedOn { get; set; }

    public DateTime? CreatedOn { get; set; }
}

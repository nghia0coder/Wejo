using Wejo.Common.Domain.Entities.Sports;
using Wejo.Common.Domain.Entities.Venues;

namespace Wejo.Common.Domain.Entities.Games;

public partial class Game
{
    public Guid Id { get; set; }

    public int SportId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public int? SportFormatId { get; set; }

    public Guid? VenueId { get; set; }

    public int? GameTypeId { get; set; }

    public string Area { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool GameAccess { get; set; }

    public bool BringEquipment { get; set; }

    public bool CostShared { get; set; }

    public bool GameSkill { get; set; }

    public int? SkillStart { get; set; }

    public int? SkillEnd { get; set; }

    public int? TotalPlayer { get; set; }

    public int? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<GamePlayerDetail> GamePlayerDetails { get; set; } = new List<GamePlayerDetail>();

    public virtual GameType? GameType { get; set; }

    public virtual Sport Sport { get; set; } = null!;

    public virtual SportFormat? SportFormat { get; set; }

    public virtual Venue? Venue { get; set; }
}

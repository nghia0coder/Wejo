namespace Wejo.Game.Application.Request;

using Common.Core.Requests;

public class GameCreateR : BaseR
{
    public int SportId { get; set; }
    public string CreatedBy { get; set; } = null!;
    public int? SportFormatId { get; set; }
    public Guid? VenueId { get; set; }
    public int? GameTypeId { get; set; }
    public string Area { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool GameAccess { get; set; }
    public bool BringEquipment { get; set; }
    public bool CostShared { get; set; }
    public bool GameSkill { get; set; }
    public int? SkillStart { get; set; }
    public int? SkillEnd { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int? TotalPlayer { get; set; }
    public string? Description { get; set; }
}

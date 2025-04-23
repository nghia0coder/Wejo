using Wejo.Common.Core.Enums;
using Wejo.Common.SeedWork.Dtos;

namespace Wejo.Common.Domain.Entities;

partial class Game
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public Game()
    {
        CreatedAt = DateTime.UtcNow;
    }


    public static Game Create(
        int sportId,
        string createdBy,
        int? sportFormatId,
        Guid? venueId,
        int? gameTypeId,
        string area,
        DateTime startTime,
        DateTime endTime,
        bool gameAccess,
        bool bringEquipment,
        bool costShared,
        bool gameSkill,
        int? skillStart,
        int? skillEnd,
        int? totalPlayer,
        GameStatus status,
        string? description,
        NetTopologySuite.Geometries.Point location)
    {

        var res = new Game
        {
            Id = Guid.NewGuid(),
            SportId = sportId,
            CreatedBy = createdBy,
            SportFormatId = sportFormatId,
            VenueId = venueId,
            GameTypeId = gameTypeId,
            Area = area,
            StartTime = startTime,
            EndTime = endTime,
            GameAccess = gameAccess,
            BringEquipment = bringEquipment,
            CostShared = costShared,
            GameSkill = gameSkill,
            SkillStart = skillStart,
            SkillEnd = skillEnd,
            TotalPlayer = totalPlayer,
            Status = status,
            Description = description,
            Location = location
        };

        return res;
    }

    public ViewDto ToViewDto()
    {
        var res = ToBaseDto<ViewDto>();

        return res;
    }

    public T ToBaseDto<T>() where T : BaseDto, new()
    {
        return new T
        {
            Id = Id,
            SportId = (SportType)SportId,
            CreatedBy = CreatedBy,
            SportFormatId = SportFormatId,
            VenueId = VenueId,
            GameTypeId = GameTypeId,
            Area = Area,
            GameAccess = GameAccess,
            GameSkill = GameSkill,
            SkillStart = SkillStart,
            SkillEnd = SkillEnd,
            TotalPlayer = TotalPlayer,
            Status = (int?)Status,
            Description = Description
        };
    }

    #endregion

    #region -- Classes --

    public class BaseDto : IdDto
    {
        #region -- Properties --

        public string SportName => SportId?.ToString() ?? string.Empty;

        public SportType? SportId { get; set; }

        public string CreatedBy { get; set; } = null!;

        public int? SportFormatId { get; set; }

        public Guid? VenueId { get; set; }

        public int? GameTypeId { get; set; }

        public string Area { get; set; } = null!;
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool GameAccess { get; set; }

        public bool GameSkill { get; set; }

        public int? SkillStart { get; set; }

        public int? SkillEnd { get; set; }

        public int? TotalPlayer { get; set; }

        public int? Status { get; set; }

        public string? Description { get; set; }

        #endregion
    }

    public class SearchDto : BaseDto
    {
        public string? PlayerAvatarJson { get; set; }

        //public List<string> PlayerAvatar =>
        //    string.IsNullOrWhiteSpace(PlayerAvatarJson) ? new List<string>() :
        //    JsonConvert.DeserializeObject<List<string>>(PlayerAvatarJson);

        public int CurrentPlayer { get; set; }
        public int SlotLeft { get; set; }
        public double Distance { get; set; }
    }

    public class ViewDto : BaseDto
    {
    }



    #endregion
}

namespace Wejo.Common.Domain.Entities;

using Common.Core.Enums;
using SeedWork.Dtos;

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
        DateOnly date,
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
            Date = date,
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

    /// <summary>
    /// Convert to data transfer object
    /// </summary>
    /// <returns>Return the DTO</returns>
    public ViewDto ToViewDto()
    {
        var res = ToBaseDto<ViewDto>();

        return res;
    }

    /// <summary>
    /// Convert to data transfer object
    /// </summary>
    /// <returns>Return the DTO</returns>
    public T ToBaseDto<T>() where T : BaseDto, new()
    {
        return new T
        {
            Id = Id,
            SportId = SportId,
            CreatedBy = CreatedBy,
            SportFormatId = SportFormatId,
            VenueId = VenueId,
            GameTypeId = GameTypeId,
            Area = Area,
            Date = Date,
            StartTime = StartTime,
            EndTime = EndTime,
            GameAccess = GameAccess,
            BringEquipment = BringEquipment,
            CostShared = CostShared,
            GameSkill = GameSkill,
            SkillStart = SkillStart,
            SkillEnd = SkillEnd,
            TotalPlayer = TotalPlayer,
            Status = Status,
            Description = Description
        };
    }

    #endregion

    #region -- Classes --

    /// <summary>
    /// Base
    /// </summary>
    public class BaseDto : IdDto
    {
        #region -- Properties --

        public int? SportId { get; set; }

        public string CreatedBy { get; set; } = null!;

        public int? SportFormatId { get; set; }
        public Guid? VenueId { get; set; }
        public int? GameTypeId { get; set; }
        public string Area { get; set; } = null!;
        public DateOnly Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool GameAccess { get; set; }
        public bool BringEquipment { get; set; }
        public bool CostShared { get; set; }
        public bool GameSkill { get; set; }
        public int? SkillStart { get; set; }
        public int? SkillEnd { get; set; }
        public int? TotalPlayer { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public GameStatus? Status { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }

        #endregion
    }

    /// <summary>
    /// Search
    /// </summary>
    public class SearchDto : BaseDto
    {
    }

    /// <summary>
    /// View
    /// </summary>
    public class ViewDto : BaseDto
    {
    }

    /// <summary>
    /// Profile
    /// </summary>
    public class ProfileDto : IdDto
    {
        #region -- Properties --

        /// <summary>
        /// UserName
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// ProfileName
        /// </summary>
        public string? ProfileName { get; set; }

        #endregion
    }

    #endregion
}

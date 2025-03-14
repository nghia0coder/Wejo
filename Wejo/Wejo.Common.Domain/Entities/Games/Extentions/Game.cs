namespace Wejo.Common.Domain.Entities;

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


    /// <summary>
    /// Create
    /// </summary>
    /// <param name="sportId"></param>
    /// <param name="createdBy"></param>
    /// <param name="sportFormatId"></param>
    /// <param name="venueId"></param>
    /// <param name="gameTypeId"></param>
    /// <param name="area"></param>
    /// <param name="date"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="gameAccess"></param>
    /// <param name="bringEquipment"></param>
    /// <param name="costShared"></param>
    /// <param name="gameSkill"></param>
    /// <param name="skillStart"></param>
    /// <param name="skillEnd"></param>
    /// <param name="totalPlayer"></param>
    /// <param name="status"></param>
    /// <param name="description"></param>
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
        int? status,
        string? description)
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
            Description = description
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

        /// <summary>
        /// SportId
        /// </summary>
        public int? SportId { get; set; }

        /// <summary>
        /// CreatedBy
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// SportFormatId
        /// </summary>
        public int? SportFormatId { get; set; }

        /// <summary>
        /// VenueId
        /// </summary>
        public Guid? VenueId { get; set; }

        /// <summary>
        /// GameTypeId
        /// </summary>
        public int? GameTypeId { get; set; }

        /// <summary>
        /// Area
        /// </summary>
        public string Area { get; set; } = null!;

        /// <summary>
        /// Date
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// GameAccess
        /// </summary>
        public bool GameAccess { get; set; }

        /// <summary>
        /// BringEquipment
        /// </summary>
        public bool BringEquipment { get; set; }

        /// <summary>
        /// CostShared
        /// </summary>
        public bool CostShared { get; set; }

        /// <summary>
        /// GameSkill
        /// </summary>
        public bool GameSkill { get; set; }

        /// <summary>
        /// SkillStart
        /// </summary>
        public int? SkillStart { get; set; }

        /// <summary>
        /// SkillEnd
        /// </summary>
        public int? SkillEnd { get; set; }

        /// <summary>
        /// TotalPlayer
        /// </summary>
        public int? TotalPlayer { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }

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

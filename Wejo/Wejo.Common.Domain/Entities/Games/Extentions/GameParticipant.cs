namespace Wejo.Common.Domain.Entities;

using Core.Enums;

partial class GameParticipant
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public GameParticipant()
    {
        CreatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="gameId"></param>
    /// <param name="gameId"></param>
    public static GameParticipant Create(Guid gameId, string userId, PlayerStatus playerStatus)
    {
        var res = new GameParticipant
        {
            GameId = gameId,
            UserId = userId,
            Status = playerStatus,
            JoinedAt = DateTime.UtcNow
        };

        return res;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="playerStatus"></param>
    public void Update(PlayerStatus playerStatus)
    {
        Status = playerStatus;

        ModifiedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Convert to data transfer object
    /// </summary>
    /// <returns>Return the DTO</returns>
    public SearchDto ToSearchDto()
    {
        var res = ToBaseDto<SearchDto>();

        res.UserId = UserId;
        res.UserName = User?.FirstName + User?.LastName;
        res.Level = User?.Level.ToString();
        res.CreatedOn = CreatedOn;
        res.IsHost = Game?.CreatedBy == UserId;

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
            UserId = UserId,
            GameId = GameId,
            PlayerStatus = Status.ToString(),
            JoinedAt = JoinedAt,
        };
    }

    #endregion

    #region -- Classes --

    /// <summary>
    /// Base
    /// </summary>
    public class BaseDto
    {
        #region -- Properties --

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// GameId
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// PlayerStatus
        /// </summary>
        public string? PlayerStatus { get; set; }

        /// <summary>
        /// JoinedAt
        /// </summary>
        public DateTime? JoinedAt { get; set; }


        #endregion
    }

    /// <summary>
    /// Search
    /// </summary>
    public class SearchDto : BaseDto
    {

        /// <summary>
        /// UserName
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// IsHost
        /// </summary>
        public bool? IsHost { get; set; }
    }

    /// <summary>
    /// View
    /// </summary>
    public class ViewDto : BaseDto
    {
    }

    #endregion
}

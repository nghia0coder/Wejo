namespace Wejo.Common.Domain.Entities;

using Core.Enums;
using SeedWork.Dtos;

partial class Notification
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public Notification()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="type"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="relatedEntityId"></param>
    /// <param name="isRead"></param>
    public static Notification Create(string userId, NotificationType type, string title, string message, Guid relatedEntityId, bool isRead, bool isSeen)
    {
        var res = new Notification
        {
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            RelatedEntityId = relatedEntityId,
            IsRead = isRead,
            IsSeen = isSeen,
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
            Content = Message,
            Type = Type.ToString(),
            IsRead = IsRead,
            IsSeen = IsSeen,
            CreatedOn = CreatedOn,
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
        /// Notification content
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// Notification type
        /// </summary>
        public string Type { get; set; } = null!;

        /// <summary>
        /// Is read ?
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Is seen ?
        /// </summary>
        public bool IsSeen { get; set; }

        /// <summary>
        /// The time when the notification was created
        /// </summary>
        public DateTime CreatedOn { get; set; }

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

    }

    #endregion
}

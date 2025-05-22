namespace Wejo.Common.Domain.Entities;
partial class UserPlaypal
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserPlaypal()
    {
        CreatedOn = DateTime.UtcNow;
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
            CreatedOn = CreatedOn
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
        /// CreatedOn
        /// </summary>
        public DateTime? CreatedOn { get; set; }

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

    #endregion
}

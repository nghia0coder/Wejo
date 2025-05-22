namespace Wejo.Common.Core.Filters;

/// <summary>
/// Base filter
/// </summary>
public class BaseFilter
{
    #region -- Classes --

    /// <summary>
    /// Search
    /// </summary>
    public class Search
    {
        #region -- Properties --

        /// <summary>
        /// List Name
        /// </summary>
        public List<string>? Names { get; set; }

        /// <summary>
        /// Keyword
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string? UserName { get; set; }

        #endregion
    }

    #endregion
}

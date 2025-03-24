namespace Wejo.Identity.Application.Filters;

using Common.Core.Enums;
using Common.Core.Filters;

/// <summary>
/// Filter
/// </summary>
public class UserPlaypalFilter : BaseFilter
{
    #region -- Classes --

    /// <summary>
    /// Search
    /// </summary>
    public new class Search : BaseFilter.Search
    {
        /// <summary>
        /// UserStatus
        /// </summary>
        public List<SportType>? SportType { get; set; }
    }

    #endregion
}

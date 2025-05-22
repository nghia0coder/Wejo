namespace Wejo.Game.Application.Filters;

using Common.Core.Enums;
using Common.Core.Filters;

/// <summary>
/// Filter
/// </summary>
public class PlayerFilter : BaseFilter
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
        public PlayerStatus Status { get; set; }
    }

    #endregion
}

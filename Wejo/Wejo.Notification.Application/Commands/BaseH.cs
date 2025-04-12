namespace Wejo.Notification.Application.Commands;

using Common.Domain.Interfaces;

/// <summary>
/// Base handler
/// </summary>
/// <remarks>
/// Initialize
/// </remarks>
/// <param name="context">DB context</param>
public abstract class BaseH(IWejoContext context)
{
    #region -- Fields --

    /// <summary>
    /// DB context
    /// </summary>
    protected readonly IWejoContext _context = context;

    #endregion
}

/// <summary>
/// Base handler
/// </summary>
/// <remarks>
/// Initialize
/// </remarks>
/// <param name="context">DB context</param>
/// <param name="setting">Setting</param>
public abstract class BaseSettingH(IWejoContext context) : BaseH(context)
{
}

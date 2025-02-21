namespace Wejo.Identity.Application.Commands;

using Common.Domain.Interfaces;
using Interfaces;

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
public abstract class BaseSettingH(IWejoContext context, ISetting setting) : BaseH(context)
{

    #region -- Fields --

    /// <summary>
    /// Setting
    /// </summary>
    protected readonly ISetting _setting = setting;

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
/// <param name="rs">Redis store</param>
public abstract class BaseRedisH(IWejoContext context, ISetting setting) : BaseSettingH(context, setting)
{
}

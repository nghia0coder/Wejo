namespace Wejo.Background.Job.Services;

using Common.Domain.Interfaces;
using Interfaces;

/// <summary>
/// Base service
/// </summary>
/// <remarks>
/// Initialize
/// </remarks>
/// <param name="context">DB context</param>
public abstract class BaseService(IWejoContext context)
{
    #region -- Fields --

    /// <summary>
    /// DB context
    /// </summary>
    protected readonly IWejoContext _context = context;

    #endregion
}

/// <summary>
/// Base service
/// </summary>
/// <remarks>
/// Initialize
/// </remarks>
/// <param name="context">DB context</param>
/// <param name="setting">Setting</param>
public abstract class BaseSettingS(IWejoContext context, ISetting setting) : BaseService(context)
{
    #region -- Fields --

    /// <summary>
    /// Setting
    /// </summary>
    protected readonly ISetting _setting = setting;

    #endregion
}

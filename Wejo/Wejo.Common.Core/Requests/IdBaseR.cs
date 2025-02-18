using Microsoft.AspNetCore.Http;

namespace Wejo.Common.Core.Requests;

using SeedWork.Interfaces;

/// <summary>
/// IdBase request
/// </summary>
public class IdBaseR : BaseR, IEntityId<Guid>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public IdBaseR() { }

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="hc">HTTP context</param>
    public IdBaseR(HttpContext hc) : base(hc) { }

    #endregion

    #region -- Implements --

    /// <summary>
    /// Id
    /// </summary>
    public virtual Guid Id { get; set; }

    #endregion

    #region -- Properties --

    /// <summary>
    /// HashId
    /// </summary>
    public virtual string? HashId { get; set; }

    #endregion
}

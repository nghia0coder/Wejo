using Wejo.Common.Core.Requests;

namespace Wejo.Identity.Application.Requests;

/// <summary>
/// Request
/// </summary>
public class UserViewR : BaseR
{
    /// <summary>
    /// Id
    /// </summary>
    public string? Id { get; set; }
}

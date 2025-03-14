namespace Wejo.Identity.Application.Requests;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class UserCheckExistR : BaseR
{
    /// <summary>
    /// Id
    /// </summary>
    public string? Id { get; set; }
}

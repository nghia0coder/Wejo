namespace Wejo.Identity.Application.Requests;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class UserLoginSocialR : BaseR
{
    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// IsNewUser
    /// </summary>
    public bool IsNewUser { get; set; }

    /// <summary>
    /// LocalId
    /// </summary>
    public string? LocalId { get; set; }
}


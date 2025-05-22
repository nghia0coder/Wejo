namespace Wejo.Identity.Application.Requests;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class UserSendOtpR : BaseR
{
    /// <summary>
    /// PhoneNumber
    /// </summary>
    public string? PhoneNumber { get; set; }
}

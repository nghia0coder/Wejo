namespace Wejo.Identity.Application.Requests;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class UserVerifyOtpR : BaseR
{
    /// <summary>
    /// SessionId
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// OtpCode
    /// </summary>
    public string? OtpCode { get; set; }
}

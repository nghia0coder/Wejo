namespace Wejo.Identity.Application.Interfaces;

using Common.SeedWork.Dtos;

/// <summary>
/// Interface FirebaseService
/// </summary>
public interface IFirebaseAuthService
{
    /// <summary>
    /// Send Otp via Firebase Auth
    /// </summary>
    /// <param name="phoneNumber">Request</param>
    /// <returns>Return the result</returns>
    Task<FirebaseSessionDto> SendOtpAsync(string phoneNumber);

    /// <summary>
    /// Send Otp via Firebase Auth
    /// </summary>
    /// <param name="sessionInfo">Session Info</param>
    /// <param name="otpCode">Otp from user</param>
    /// <returns>Return the result</returns>
    Task<JwtDto> VerifyOtpAsync(string sessionInfo, string otpCode);

    /// <summary>
    /// Login with Google
    /// </summary>
    /// <param name="idToken">Session Info</param>
    /// <returns>Return the result</returns>

    Task<JwtDto> SignInWithGoogleAsync(string idToken);
}

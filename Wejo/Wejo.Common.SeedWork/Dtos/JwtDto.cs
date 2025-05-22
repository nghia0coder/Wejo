namespace Wejo.Common.SeedWork.Dtos;

/// <summary>
/// JWT data transfer object
/// </summary>
public class JwtDto
{
    #region -- Properties --

    /// <summary>
    /// Access Token
    /// </summary>
    public string IdToken { get; set; } = string.Empty;

    /// <summary>
    /// RefreshToken
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// JWT signing
    /// </summary>
    public string ExpiresIn { get; set; } = string.Empty;

    /// <summary>
    /// User Id
    /// </summary>
    public string LocalId { get; set; } = string.Empty;

    /// <summary>
    /// IsNewUser
    /// </summary>
    public bool IsNewUser { get; set; }

    /// <summary>
    /// PhoneNumber
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    #endregion

    #region -- Fields --

    /// <summary>
    /// Time to live of AccessToken
    /// </summary>
    private int _timeAt;

    /// <summary>
    /// Time to live of RefreshToken
    /// </summary>
    private int _timeRt;

    #endregion
}

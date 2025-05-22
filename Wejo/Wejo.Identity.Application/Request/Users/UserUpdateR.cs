using Microsoft.AspNetCore.Http;

namespace Wejo.Identity.Application.Requests;

using Common.Core.Requests;

/// <summary>
/// Request
/// </summary>
public class UserUpdateR : BaseR
{
    /// <summary>
    /// FirstName
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// LastName
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gender
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Bio
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// Image Url
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Image
    /// </summary>
    public IFormFile? Image { get; set; }

}

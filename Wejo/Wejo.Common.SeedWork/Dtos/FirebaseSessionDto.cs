using System.Text.Json.Serialization;

namespace Wejo.Common.SeedWork.Dtos;


/// <summary>
/// FireBase Session data transfer object
/// </summary>
public class FirebaseSessionDto
{
    /// <summary>
    /// SessionInfo
    /// </summary>
    [JsonPropertyName("sessionInfo")]
    public string? SessionInfo { get; set; }
}

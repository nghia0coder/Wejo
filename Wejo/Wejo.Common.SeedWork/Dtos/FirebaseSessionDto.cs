using System.Text.Json.Serialization;

namespace Wejo.Common.SeedWork.Dtos;


/// <summary>
/// FireBase Session data transfer object
/// </summary>
public class FirebaseSessionDto
{
    /// <summary>
    /// SessionId
    /// </summary>
    [JsonPropertyName("SessionId")]
    public string? SessionId { get; set; }
}

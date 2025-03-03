using Newtonsoft.Json;

namespace Wejo.Common.SeedWork.Dtos;


/// <summary>
/// FireBase Session data transfer object
/// </summary>
public class FirebaseSessionDto
{
    /// <summary>
    /// SessionId
    /// </summary>
    [JsonProperty("sessionInfo")]
    public string? SessionId { get; set; }
}

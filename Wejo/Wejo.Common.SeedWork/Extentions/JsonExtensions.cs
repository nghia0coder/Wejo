using Newtonsoft.Json;

namespace Wejo.Common.Extensions;

/// <summary>
/// Provides extension methods for converting objects to JSON strings and vice versa.
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    /// <summary>
    /// Deserializes a JSON string to an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize into.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>An object of type T deserialized from the JSON string.</returns>
    public static T FromJson<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}

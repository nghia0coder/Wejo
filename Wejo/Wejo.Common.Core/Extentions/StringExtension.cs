using Newtonsoft.Json;
using System.Collections;

namespace Wejo.Common.Core.Extensions;
/// <summary>
/// String extension for using [this string] only
/// </summary>
public static class StringExtension
{

    #region -- Converts --

    /// <summary>
    /// Convert environment variables to an object
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    /// <param name="projectPrefix">The prefix for service-specific environment variables</param>
    /// <param name="commonPrefix">The prefix for common environment variables</param>
    /// <param name="splitter">Splitter for a variable</param>
    /// <returns>Return an object of type T</returns>
    public static T ConvertEnvironmentVariable<T>(this string projectPrefix, string commonPrefix, char splitter = '_') where T : new()
    {
        if (string.IsNullOrWhiteSpace(projectPrefix))
        {
            return new T();
        }

        var lines = new List<string>();
        var prefix = commonPrefix;

        // Correct prefix for common
        var arrays = commonPrefix.Split(splitter);
        if (arrays.Length > 0)
        {
            prefix = arrays[0] + splitter + projectPrefix;
        }

        var variables = Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry i in variables)
        {
            // Variables that do not begin with the specified prefix will be skipped
            var key = i.Key + string.Empty;
            if (!key.StartsWith(prefix) && !key.StartsWith(commonPrefix))
            {
                continue;
            }

            // Remove prefix and splitter (first data level)
            var t = string.Format("{0}={1}", i.Key, i.Value);
            t = t.Replace(prefix + splitter, string.Empty);
            t = t.Replace(commonPrefix + splitter, string.Empty);

            lines.Add(t);
        }

        var dictionary = lines.ToArray().ConvertIniToDictionary(splitter);
        var json = JsonConvert.SerializeObject(dictionary);
        if (json == null)
        {
            return new T();
        }

        var res = JsonConvert.DeserializeObject<T>(json);
        if (res == null)
        {
            res = new T();
        }

        return res!;
    }

    /// <summary>
    /// Convert INI format to dictionary
    /// </summary>
    /// <param name="lines">Array of environment variables</param>
    /// <param name="splitter">Splitter for a variable</param>
    /// <param name="equality">Equality symbol used with variables</param>
    /// <returns>Return a dictionary</returns>
    private static Dictionary<string, object> ConvertIniToDictionary(this string[] lines, char splitter = '.', char equality = '=')
    {
        var res = new Dictionary<string, object>();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (!string.IsNullOrEmpty(trimmedLine) && trimmedLine.Contains(equality))
            {
                var parts = trimmedLine.Split(equality);
                var keys = parts[0].Split(splitter);
                var value = trimmedLine.Replace($"{parts[0]}{equality}", "");
                var currentLevel = res;

                for (int i = 0; i < keys.Length - 1; i++)
                {
                    var key = keys[i];
                    if (!currentLevel.ContainsKey(key))
                    {
                        currentLevel[key] = new Dictionary<string, object>();
                    }

                    currentLevel = (Dictionary<string, object>)currentLevel[key];
                }

                var lastKey = keys[keys.Length - 1];
                currentLevel[lastKey] = value;
            }
        }

        return res;
    }

    #endregion
}

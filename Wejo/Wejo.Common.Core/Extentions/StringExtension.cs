using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

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

    /// <summary>
    /// Convert from JSON to instance of T
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="s">JSON data</param>
    /// <returns>Return to instance of T</returns>
    public static T? ToInstNull<T>(this string? s) where T : new()
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return default;
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
        catch (Exception ex)
        {
            var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }

        return default;
    }

    /// <summary>
    /// Convert a string UID to a hashed long value
    /// </summary>
    /// <param name="uid">The UID string</param>
    /// <returns>Return to instance of T</returns>
    public static long ToUid(this string uid)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(uid));
            return BitConverter.ToInt64(hashBytes, 0);
        }
    }

    #endregion
}

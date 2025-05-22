namespace Wejo.Common.SeedWork;

using Dtos;
using SeedWork.Interfaces;
using static Dtos.ConnectionDto;

/// <summary>
/// Setting base
/// </summary>
public class SettingBase : ISettingBase
{
    #region -- Implements --

    /// <summary>
    /// Project prefix
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// Environment (local, dev, uat, stg and pro)
    /// </summary>
    public string Environment { get; set; }

    /// <summary>
    /// Domain
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// Swagger enabled
    /// </summary>
    public bool SwaggerEnabled { get; set; }

    /// <summary>
    /// Development mode
    /// </summary>
    public bool DevMode { get; set; }

    /// <summary>
    /// Protocols (An example such as 'Http1_80;Http2_81' means that Http1 runs on port 80 and Http2 runs on port 81)
    /// </summary>
    public string? Protocols { get; set; }

    /// <summary>
    /// Information
    /// </summary>
    public string? Information
    {
        get
        {
            return !IsProduction ? $"{Environment.ToUpper()}" : null;
        }
    }

    /// <summary>
    /// Is production mode
    /// </summary>
    public bool IsProduction
    {
        get
        {
            var t = Environment.ToLower();
            return t == "pro" || t == "production";
        }
    }

    /// <summary>
    /// Is local mode
    /// </summary>
    public bool IsLocal => Environment.Equals("local", StringComparison.CurrentCultureIgnoreCase);

    /// <summary>
    /// JSON Web Token
    /// </summary>
    public JwtDto Jwt { get; }

    /// <summary>
    /// Database
    /// </summary>
    public DatabaseDto Db { get; }


    /// <summary>
    /// The origins that are allowed (CORS)
    /// </summary>
    public string? Origins { get; set; }

    /// <summary>
    /// Encrypt key
    /// </summary>
    public string EncryptKey { get; set; }

    #endregion

    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public SettingBase()
    {
        Prefix = string.Empty;
        Environment = string.Empty;
        Domain = string.Empty;

        Jwt = new JwtDto();
        Db = new DatabaseDto();
        EncryptKey = string.Empty;
    }

    #endregion

    #region -- Properties --

    /// <summary>
    /// X API key
    /// </summary>
    public static string? XApiKey { get; set; }

    /// <summary>
    /// Development mode
    /// </summary>
    public static bool DevelopmentMode { get; set; }

    #endregion

}

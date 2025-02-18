namespace Wejo.Common.SeedWork.Interfaces;

using Dtos;
using static Dtos.ConnectionDto;

/// <summary>
/// Interface setting base
/// </summary>
public interface ISettingBase
{
    #region -- Properties --

    /// <summary>
    /// Project prefix
    /// </summary>
    string Prefix { get; set; }

    /// <summary>
    /// Environment (local, dev, uat, stg and pro)
    /// </summary>
    string Environment { get; set; }

    /// <summary>
    /// Domain
    /// </summary>
    string Domain { get; set; }

    /// <summary>
    /// Swagger enabled
    /// </summary>
    bool SwaggerEnabled { get; set; }

    /// <summary>
    /// Development mode
    /// </summary>
    bool DevMode { get; set; }

    /// <summary>
    /// Protocols (An example such as 'Http1_80;Http2_81' means that Http1 runs on port 80 and Http2 runs on port 81)
    /// </summary>
    string? Protocols { get; set; }

    /// <summary>
    /// Information
    /// </summary>
    public string? Information { get; }

    /// <summary>
    /// Is production mode
    /// </summary>
    bool IsProduction { get; }

    /// <summary>
    /// Is local mode
    /// </summary>
    bool IsLocal { get; }

    /// <summary>
    /// JSON Web Token
    /// </summary>
    JwtDto Jwt { get; }

    /// <summary>
    /// Database
    /// </summary>
    DatabaseDto Db { get; }

    /// <summary>
    /// The origins that are allowed (CORS)
    /// </summary>
    string? Origins { get; set; }

    /// <summary>
    /// Encrypt key
    /// </summary>
    string EncryptKey { get; set; }

    #endregion
}

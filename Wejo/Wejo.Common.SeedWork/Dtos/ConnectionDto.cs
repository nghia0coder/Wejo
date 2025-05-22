namespace Wejo.Common.SeedWork.Dtos;

/// <summary>
/// Connection data transfer object
/// </summary>
public abstract class ConnectionDto
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public ConnectionDto()
    {
        Host = string.Empty;
        UserName = string.Empty;
        Password = string.Empty;
    }

    #endregion

    #region -- Properties --

    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public ushort Port { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }

    #endregion

    #region -- Classes --

    /// <summary>
    /// Database
    /// </summary>
    public class DatabaseDto : ConnectionDto
    {
        #region -- Methods --

        /// <summary>
        /// Initialize
        /// </summary>
        public DatabaseDto() : base()
        {
            Name = string.Empty;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        #endregion
    }

    #endregion
}

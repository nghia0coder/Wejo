namespace Wejo.Common.SeedWork.Dtos;

/// <summary>
/// Dictionary data transfer object
/// </summary>
public class DicDto
{
    #region -- Properties --

    /// <summary>
    /// Gets or sets the key
    /// </summary>
    /// <value>
    /// The key
    /// </value>
    public string Key { get; set; } = default!;

    /// <summary>
    /// Gets or sets the value
    /// </summary>
    /// <value>
    /// The value
    /// </value>
    public object Value { get; set; } = default!;

    #endregion
}

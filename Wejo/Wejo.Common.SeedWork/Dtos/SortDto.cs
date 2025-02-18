namespace Wejo.Common.SeedWork.Dtos;

/// <summary>
/// Sort data transfer object
/// </summary>
public class SortDto
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public SortDto() : this(string.Empty) { }

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="field">Field name</param>
    public SortDto(string field) : this(field, Ascending) { }

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="field">Field name</param>
    /// <param name="direction">Direction code [ASC or DESC]</param>
    public SortDto(string field, string direction)
    {
        Field = field;
        _direction = direction;
    }

    #endregion

    #region -- Properties --

    /// <summary>
    /// Field name
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Direction code [ASC or DESC]
    /// </summary>
    public string Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            if (Ascending != value && Descending != value)
            {
                value = Ascending;
            }

            _direction = value;
        }
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// Direction
    /// </summary>
    private string _direction;

    #endregion

    #region -- Constants --

    /// <summary>
    /// Ascending
    /// </summary>
    public const string Ascending = "ASC";

    /// <summary>
    /// Descending
    /// </summary>
    public const string Descending = "DESC";

    #endregion
}

namespace Wejo.Common.Core.Requests;

/// <summary>
/// FrToDate request
/// </summary>
public class FrToDateR : BaseR
{
    #region -- Properties --

    /// <summary>
    /// FrDate
    /// </summary>
    public DateTime FrDate { get; set; }

    /// <summary>
    /// ToDate
    /// </summary>
    public DateTime ToDate { get; set; }

    #endregion
}

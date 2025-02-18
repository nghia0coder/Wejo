namespace Wejo.Common.SeedWork.Responses;

/// <summary>
/// Multiple response
/// </summary>
public class MultipleResponse : SingleResponse
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public MultipleResponse() : base() { }

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="request">Request</param>
    public MultipleResponse(object? request) : base(request) { }

    /// <summary>
    /// Set success
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="data">Data</param>
    public void SetSuccess(string key, object? data)
    {
        if (Data == null)
        {
            Data = [];
        }

        if (Data.ContainsKey(key))
        {
            Data[key] = data;
        }
        else
        {
            Data.Add(key, data);
        }
    }

    #endregion

    #region -- Properties --

    /// <summary>
    /// Data
    /// </summary>
    public new Dictionary<string, object?>? Data { get; private set; }

    #endregion
}

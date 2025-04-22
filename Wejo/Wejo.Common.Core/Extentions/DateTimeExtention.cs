namespace Wejo.Common.Core.Extensions;

/// <summary>
/// Helper for date and time operations
/// </summary>
public static class DateTimeExtension
{
    /// <summary>
    /// Generates a bucket value from a date time (format YYYYMM)
    /// </summary>
    /// <param name="dateTime">The date time</param>
    /// <returns>An integer representing the bucket</returns>
    public static int GetBucket(DateTime dateTime)
    {
        return int.Parse(dateTime.ToString("yyyyMM"));
    }

    /// <summary>
    /// Generates buckets for all months in a date range
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>List of bucket values</returns>
    public static List<int> GenerateBuckets(DateTime startDate, DateTime endDate)
    {
        var buckets = new List<int>();
        for (var date = startDate; date <= endDate; date = date.AddMonths(1))
        {
            buckets.Add(GetBucket(date));
        }
        return buckets;
    }
}
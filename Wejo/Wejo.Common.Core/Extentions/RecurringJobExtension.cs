using Hangfire;
using System.Linq.Expressions;

public static class RecurringJobExtension
{
    public static void AddOrUpdateAutoId<T>(
        this IRecurringJobManager recurringJobManager,
        Expression<Func<T, Task>> methodCall,
        string cronExpression,
        TimeZoneInfo timeZone = null,
        RecurringJobOptions options = null)
    {
        if (!(methodCall.Body is MethodCallExpression methodCallExpression))
            throw new ArgumentException("Biểu thức phải là một lời gọi phương thức", nameof(methodCall));


        string methodName = methodCallExpression.Method.Name;
        string jobId = $"{typeof(T).FullName}.{methodName}";

        recurringJobManager.AddOrUpdate<T>(jobId, methodCall, cronExpression, options);
    }
}

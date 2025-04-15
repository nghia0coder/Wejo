using Cassandra;
using Microsoft.Extensions.DependencyInjection;

namespace Wejo.Game.Application.Extensions;

/// <summary>
/// DI extension
/// </summary>
public static class DiCassandraExtensions
{
    #region -- Methods --

    /// <summary>
    /// Add DI for Cassandra
    /// </summary>
    /// <param name="services">Service</param>
    public static IServiceCollection AddCassandra(this IServiceCollection services)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint("localhost")
            .WithPort(9042)
            .Build();
        var session = cluster.Connect("wejo");

        services.AddSingleton<ISession>(session);
        return services;
    }

    #endregion
}

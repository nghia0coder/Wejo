using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wejo.Game.Application.Extensions;

using Common.SeedWork;
using Interfaces;
using Services;

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
    public static IServiceCollection AddCassandra(this IServiceCollection services, IConfiguration configuration)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint("cassandra") // Change to localhost if running locally
            .WithPort(9042)
            .WithRetryPolicy(new DefaultRetryPolicy())
            .Build();
        var session = cluster.Connect("wejo");

        var chatConfig = new ChatConfig();
        services.AddSingleton(chatConfig);

        services.AddSingleton<ICassandraStatementFactory, CassandraStatementFactory>();

        services.AddSingleton<ISession>(session);

        return services;
    }

    #endregion
}

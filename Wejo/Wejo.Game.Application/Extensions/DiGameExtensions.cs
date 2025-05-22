using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Wejo.Game.Application.Extensions;

using Commands;
using Common.SeedWork.Responses;
using Queries;
using Request;

/// <summary>
/// DI extension
/// </summary>
public static class DiGameExtensions
{
    #region -- Methods --

    /// <summary>
    /// Add DI for include commands and queries
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddDiGame(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddGameCommands(life);
        p.AddGameQueries(life);
    }

    /// <summary>
    /// Add commands handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddGameCommands(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddBehavior<IRequestHandler<GameCreateR, SingleResponse>, GameCreateH>(life);
        p.AddBehavior<IRequestHandler<GameListInfoR, SingleResponse>, GameListInfoH>(life);
    }

    /// <summary>
    /// Add queries handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddGameQueries(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddBehavior<IRequestHandler<GameViewR, SingleResponse>, GameViewH>(life);
    }

    #endregion
}

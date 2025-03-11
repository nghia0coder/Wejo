using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Wejo.Game.Application.Extensions;

using Commands;
using Common.SeedWork.Responses;
using Request;

/// <summary>
/// DI extension
/// </summary>
public static class DiGameParticipantExtensions
{
    #region -- Methods --

    /// <summary>
    /// Add DI for include commands and queries
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddDiGameParticipant(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddGameParticipantCommands(life);
        p.AddGameParticipantQueries(life);
    }

    /// <summary>
    /// Add commands handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddGameParticipantCommands(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddBehavior<IRequestHandler<GameParticipantCreateR, SingleResponse>, GameParticipantCreateH>(life);
        p.AddBehavior<IRequestHandler<GameParticipantUpdateR, SingleResponse>, GameParticipantUpdateH>(life);
        p.AddBehavior<IRequestHandler<GameParticipantViewR, SingleResponse>, GameParticipantViewH>(life);
    }

    /// <summary>
    /// Add queries handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddGameParticipantQueries(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
    }

    #endregion
}

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
public static class DiGameChatExtensions
{
    #region -- Methods --

    /// <summary>
    /// Add DI for include commands and queries
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddDiGameChat(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddGameChatCommands(life);
        p.AddGameChatQueries(life);
    }

    /// <summary>
    /// Add commands handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddGameChatCommands(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddBehavior<IRequestHandler<GameChatSendMessageR, SingleResponse>, GameChatSendMessageH>(life);
    }

    /// <summary>
    /// Add queries handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddGameChatQueries(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddBehavior<IRequestHandler<GameChatGetMessageR, SingleResponse>, GameChatGetMessageH>(life);
    }

    #endregion
}

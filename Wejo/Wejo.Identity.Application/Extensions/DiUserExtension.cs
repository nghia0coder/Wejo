using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Wejo.Identity.Application.Extensions;

using Commands;
using Common.SeedWork.Responses;
using Queries;
using Requests;

/// <summary>
/// DI extension
/// </summary>
public static class DiUserExtension
{
    #region -- Methods --

    /// <summary>
    /// Add DI for include commands and queries
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddDiUser(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddUserCommands(life);
        p.AddUserQueries(life);
    }

    /// <summary>
    /// Add commands handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddUserCommands(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddBehavior<IRequestHandler<UserChatSendMessageR, SingleResponse>, UserChatSendMessageH>(life);
        p.AddBehavior<IRequestHandler<UserSendOtpR, SingleResponse>, UserSendOtpH>(life);
        p.AddBehavior<IRequestHandler<UserVerifyOtpR, SingleResponse>, UserVerifyOtpH>(life);
        p.AddBehavior<IRequestHandler<UserUpdateR, SingleResponse>, UserUpdateH>(life);
        p.AddBehavior<IRequestHandler<UserLoginSocialR, SingleResponse>, UserLoginSocialH>(life);
        p.AddBehavior<IRequestHandler<UserCreateR, SingleResponse>, UserCreateH>(life);
    }

    /// <summary>
    /// Add queries handler
    /// </summary>
    /// <param name="p">MediatRServiceConfiguration</param>
    /// <param name="life">ServiceLifetime</param>
    public static void AddUserQueries(this MediatRServiceConfiguration p, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        p.AddBehavior<IRequestHandler<UserPlaypalViewR, SingleResponse>, UserPlaypalViewH>(life);
        p.AddBehavior<IRequestHandler<UserViewR, SingleResponse>, UserViewH>(life);
        p.AddBehavior<IRequestHandler<UserCheckExistR, SingleResponse>, UserCheckExistH>(life);
    }

    #endregion
}

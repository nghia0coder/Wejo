using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Realtime.API.Services;

using Common.Core.Constants;
using Common.Core.Enums;
using Common.Core.Extentions;
using Common.Core.Protos;
using Common.Domain.Entities;
using Common.Domain.Interfaces;
using Hubs;
using static Common.Core.Protos.GameParticipantService;

public class GameParticipantServiceImpl : GameParticipantServiceBase, IBaseService
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    /// <param name="hc">Notification hub</param>
    public GameParticipantServiceImpl(IWejoContext context, IHubContext<NotificationHub> hc)
    {
        _context = context;
        _hc = hc;
    }

    public override async Task<EmptyResponse> SendGameJoinRequest(GameJoinRequest request, ServerCallContext context)
    {
        var gameId = Guid.Parse(request.GameId);
        var requesterName = await _context.Users
            .Where(u => u.Id == request.RequesterId)
            .Select(u => u.FirstName + " " + u.LastName)
            .FirstOrDefaultAsync();
        var gameInfo = await _context.Games
            .Where(g => g.Id == gameId)
            .Include(g => g.Sport)
            .Select(g => new { g.Sport.Name, g.StartTime })
            .FirstOrDefaultAsync();

        var type = NotificationType.Game;
        var (title, messageTemplate) = MessageExtension.Templates[type];
        var messageFormat = string.Format(messageTemplate, requesterName, gameInfo!.Name, gameInfo.StartTime);

        var ett = Notification.Create(request.HostId, type, title, messageFormat, gameId, false, false);

        _context.Notifications.Add(ett);
        await _context.SaveChangesAsync(default);

        await _hc.Clients.Group(request.HostId)
            .SendAsync(RealTimeTopic.ReceiveNotification, new
            {
                ett.Id,
                ett.UserId,
                Type = ett.Type.ToString(),
                ett.Title,
                ett.Message,
                ett.RelatedEntityId,
                ett.IsRead,
                ett.IsSeen,
                CreatedOn = ett.CreatedOn.ToString("o")
            });

        return new EmptyResponse();
    }


    #endregion

    #region -- Fields --

    /// <summary>
    /// Context
    /// </summary>
    public IWejoContext Context => _context;

    /// <summary>
    /// Context
    /// </summary>
    private readonly IWejoContext _context;

    /// <summary>
    /// HubContext
    /// </summary>
    private readonly IHubContext<NotificationHub> _hc;

    #endregion
}

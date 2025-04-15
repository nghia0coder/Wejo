using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Notification.Application.Commands;

using Common.Core.Enums;
using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Requests;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class NotiMarkAllAsSeenH : BaseH, IRequestHandler<NotiMarkAllAsSeenR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public NotiMarkAllAsSeenH(IWejoContext context) : base(context) { }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(NotiMarkAllAsSeenR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new NotiMarkAllAsSeenV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }
        var userId = request.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return res.SetError(nameof(E119), E119);
        }

        #region -- Validate on server --
        var hasUser = await _context.Users.AnyAsync(p => p.Id == userId, cancellationToken);
        if (!hasUser)
        {
            return res.SetError(nameof(E119), E119);
        }
        #endregion

        var query = _context.Notifications.Where(n => n.UserId == userId);
        var type = request.Type.ToEnum(NotificationType.Game);

        query = query.Where(n => n.Type == type);

        var updatedCount = await query.CountAsync(cancellationToken);

        await query.ExecuteUpdateAsync(setters => setters.SetProperty(n => n.IsSeen, true), cancellationToken);

        var unseenCount = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsSeen)
            .CountAsync(cancellationToken);

        var data = new
        {
            unseenCount,
            updatedCount
        };

        return res.SetSuccess(data);
    }

    #endregion
}

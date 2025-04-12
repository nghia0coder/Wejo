using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Notification.Application.Queries;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Responses;
using Requests;
using Validators;
using Wejo.Common.Core.Enums;
using Wejo.Common.SeedWork.Extensions;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class NotificationViewH : BaseH, IRequestHandler<NotificationViewR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public NotificationViewH(IWejoContext context) : base(context) { }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(NotificationViewR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new NotificationViewV().Validate(request);
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
        var type = request.Type.ToEnum(NotificationType.GameInvitation);

        query = query.Where(n => n.Type == type);

        var notifications = await query
            .OrderByDescending(user => user.CreatedOn)
            .Select(user => user.ToViewDto())
            .ToListAsync(cancellationToken);

        var unseenCount = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsSeen)
                .CountAsync(cancellationToken);

        var data = new
        {
            notifications,
            UnseenCount = unseenCount
        };
        return res.SetSuccess(data);
    }

    #endregion
}

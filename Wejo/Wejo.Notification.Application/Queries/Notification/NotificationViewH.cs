using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Notification.Application.Queries;

using Common.Core.Enums;
using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Responses;
using Requests;
using Validators;
using Wejo.Notification.Application.Filters;
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
        var res = new SearchResponse(request.PageNum, request.PageSize, request.Paging);

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
        string? keyword = null;
        NotificationType? type = null;

        #region -- Filter --
        if (request.Filter != null)
        {
            var ft = keyword.ToInstNull<NotificationFilter.Search>();
            if (ft != null)
            {
                type = ft.Type;
            }
        }

        if (!string.IsNullOrEmpty(request.UserId))
        {
            query = query.Where(n => n.UserId == request.UserId);
        }

        if (type.HasValue)
        {
            query = query.Where(p => p.Type == type.Value);
        }

        // Paging
        res.TotalRecords = query.Count();
        if (request.Paging)
        {
            query = query.Sort(request.Sort).PageBy(request.Offset, request.PageSize);
        }
        #endregion

        var notifications = await (from noti in query select noti.ToViewDto()).ToListAsync(cancellationToken);

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

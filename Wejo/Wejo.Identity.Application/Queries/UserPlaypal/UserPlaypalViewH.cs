using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Queries;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Responses;
using Filters;
using Requests;
using Validators;
using Wejo.Common.Core.Enums;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserPlaypalViewH : BaseH, IRequestHandler<UserPlaypalViewR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public UserPlaypalViewH(IWejoContext context) : base(context) { }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserPlaypalViewR request, CancellationToken cancellationToken)
    {
        var res = new SearchResponse(request.PageNum, request.PageSize, request.Paging);

        var vr = new UserViewV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }

        #region -- Validate on server --
        var hasUser = await _context.Users.AnyAsync(p => p.Id == request.UserId, cancellationToken);
        if (!hasUser)
        {
            return res.SetError(nameof(E119), E119);
        }
        #endregion

        var q = _context.UserPlaypals
            .AsNoTracking()
            .Include(p => p.Game)
            .Where(p => p.UserId1 == request.UserId || p.UserId2 == request.UserId);

        #region -- Filter --
        string? keyword = null;
        List<SportType>? sportTypes = null;

        if (request.Filter != null)
        {
            keyword = request.Filter + "";
            var ft = keyword.ToInstNull<UserPlaypalFilter.Search>();
            if (ft != null)
            {
                keyword = ft.Keyword;
                sportTypes = ft.SportType;
            }
        }

        // Sport Type
        if (sportTypes != null && sportTypes.Count > 0)
        {
            var sportTypeIds = sportTypes.ConvertAll(x => (int)x);
            q = q.Where(p => sportTypeIds.Contains(p.Game!.SportId));
        }

        var groupedQuery = q
            .GroupBy(p => p.UserId1 == request.UserId ? p.UserId2 : p.UserId1)
            .Select(g => new
            {
                PlaypalUserId = g.Key,
                User = _context.Users
                    .Where(u => u.Id == g.Key)
                    .Select(u => new { FullName = u.FirstName + " " + u.LastName, Level = u.Level.ToString() })
                    .FirstOrDefault(),
                MutualGameCount = g.Count(),
                CreatedOn = g.Min(p => p.CreatedOn)
            });

        // Sort
        foreach (var i in request.Sort)
        {
            var descending = i.Direction.Equals(SortDto.Descending, StringComparison.OrdinalIgnoreCase);

            if (i.Field.Equals("mutualgames", StringComparison.OrdinalIgnoreCase))
            {
                groupedQuery = descending
                    ? groupedQuery.OrderByDescending(p => p.MutualGameCount)
                    : groupedQuery.OrderBy(p => p.MutualGameCount);
            }
        }

        // Paging
        res.TotalRecords = groupedQuery.Count();
        if (request.Paging)
        {
            groupedQuery = groupedQuery.Sort(request.Sort).PageBy(request.Offset, request.PageSize);
        }
        #endregion

        var data = await groupedQuery.ToListAsync(cancellationToken);

        return res.SetSuccess(data);
    }

    #endregion
}

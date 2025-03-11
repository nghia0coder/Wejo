using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Commands;

using Common.Core.Enums;
using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Filters;
using Request;
using System.Linq;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class GameParticipantViewH : BaseH, IRequestHandler<GameParticipantViewR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameParticipantViewH(IWejoContext context) : base(context)
    {
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(GameParticipantViewR request, CancellationToken cancellationToken)
    {
        var res = new SearchResponse(request.PageNum, request.PageSize, request.Paging);

        var q = _context.GameParticipants.AsNoTracking();

        #region -- Validate on client --
        var vr = new GameParticipantViewV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }
        #endregion

        var userId = request.UserId;
        if (userId == null)
        {
            return res.SetError(nameof(E119), E119);
        }

        #region -- Validate on server --
        var hasUser = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);
        if (hasUser == null)
        {
            var t = new List<DicDto> { new() { Key = nameof(userId).ToCamelCase(), Value = userId } };
            return res.SetError(nameof(E002), E002, t);
        }
        #endregion

        #region -- Filter --
        string? keyword = null;
        PlayerStatus? status = null;

        if (request.Filter != null)
        {
            keyword = request.Filter + "";
            var ft = keyword.ToInstNull<PlayerFilter.Search>();
            if (ft != null)
            {
                keyword = ft.Keyword;
                status = ft.Status;
            }
        }

        if (request.GameId != Guid.Empty)
        {
            q = q.Where(p => p.GameId == request.GameId);
        }

        // Status
        if (status.HasValue)
        {
            q = q.Where(p => p.Status == status.Value);
        }

        // Paging
        res.TotalRecords = q.Count();
        if (request.Paging)
        {
            q = q.Sort(request.Sort).PageBy(request.Offset, request.PageSize);
        }
        #endregion

        var data = await (from player in q select player.ToSearchDto()).ToListAsync(cancellationToken);

        return res.SetSuccess(data);
    }

    #endregion
}

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Commands;

using Common.Core.Enums;
using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Request;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class GameParticipantUpdateH : BaseH, IRequestHandler<GameParticipantUpdateR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameParticipantUpdateH(IWejoContext context) : base(context)
    {
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(GameParticipantUpdateR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        #region -- Validate on client --
        var vr = new GameParticipantUpdateV().Validate(request);
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
        var hasUser = await _context.Users.AnyAsync(p => p.Id == userId, cancellationToken);
        if (!hasUser)
        {
            var t = new List<DicDto> { new() { Key = nameof(userId).ToCamelCase(), Value = userId } };
            return res.SetError(nameof(E002), E002, t);
        }
        var ett = await _context.GameParticipants.Include(p => p.Game).FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (ett == null)
        {
            var t = new List<DicDto> { new() { Key = nameof(request.Id).ToCamelCase(), Value = request.Id } };
            return res.SetErrorData(nameof(E003), E003, t);
        }
        #endregion

        var playerStatus = request.PlayerStatus.ToEnum(PlayerStatus.Cancelled);
        if (playerStatus.Equals(PlayerStatus.Accepted))
        {
            var isHost = ett.Game.CreatedBy == userId;
            if (!isHost)
            {
                return res.SetError(nameof(E204), E204);
            }
        }

        ett.Update(playerStatus);

        await _context.SaveChangesAsync(cancellationToken);

        return res.SetSuccess(ett.ToViewDto());
    }

    #endregion
}

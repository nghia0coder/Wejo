using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.Application.Commands;

using Common.Core.Enums;
using Common.Core.Extensions;
using Common.Core.Protos;
using Common.Domain.Entities;
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
public class GameParticipantCreateH : BaseH, IRequestHandler<GameParticipantCreateR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public GameParticipantCreateH(IWejoContext context, GameParticipantService.GameParticipantServiceClient grpcClient) : base(context)
    {
        _grpcClient = grpcClient;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(GameParticipantCreateR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        #region -- Validate on client --
        var vr = new GameParticipantCreateV().Validate(request);
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

        var gameId = request.GameId;

        #region -- Validate on server --
        var hasUser = await _context.Users.AnyAsync(p => p.Id == userId, cancellationToken);
        if (!hasUser)
        {
            var t = new List<DicDto> { new() { Key = nameof(userId).ToCamelCase(), Value = userId } };
            return res.SetError(nameof(E002), E002, t);
        }
        var hasGamePartcipant = await _context.GameParticipants.AnyAsync(p => p.GameId == gameId && p.UserId == userId, cancellationToken);
        if (hasGamePartcipant)
        {
            var t = new List<DicDto> { new() { Key = nameof(userId).ToCamelCase(), Value = userId } };
            return res.SetError(nameof(E202), E202, t);
        }
        #endregion

        var ett = GameParticipant.Create(gameId, userId, PlayerStatus.Pending);

        await _context.GameParticipants.AddAsync(ett, cancellationToken);

        #region Send gRPC
        var hosId = await _context.Games.Where(g => g.Id == gameId).Select(g => g.CreatedBy).FirstOrDefaultAsync(cancellationToken);
        var grpcRequest = new GameJoinRequest
        {
            HostId = hosId,
            RequesterId = userId,
            GameId = gameId.ToString()
        };
        await _grpcClient.SendGameJoinRequestAsync(grpcRequest);
        #endregion

        await _context.SaveChangesAsync(default);

        return res.SetSuccess(ett.ToViewDto());
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// gRPC Client
    /// </summary>
    private readonly GameParticipantService.GameParticipantServiceClient _grpcClient;

    #endregion
}

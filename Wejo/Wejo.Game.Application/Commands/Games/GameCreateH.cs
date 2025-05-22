using MediatR;
using NetTopologySuite.Geometries;

namespace Wejo.Game.Application.Commands;

using Common.Core.Enums;
using Common.Domain.Entities;
using Common.Domain.Interfaces;
using Common.SeedWork.Responses;
using Request;

public class GameCreateH : BaseH, IRequestHandler<GameCreateR, SingleResponse>
{
    public GameCreateH(IWejoContext context) : base(context) { }

    public async Task<SingleResponse> Handle(GameCreateR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var location = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

        var ett = Game.Create(
            request.SportId,
            request.CreatedBy,
            request.SportFormatId,
            request.VenueId,
            request.GameTypeId,
            request.Area,
            request.StartTime,
            request.EndTime,
            request.GameAccess,
            request.BringEquipment,
            request.CostShared,
            request.GameSkill,
            request.SkillStart,
            request.SkillEnd,
            request.TotalPlayer,
            GameStatus.Waiting,
            request.Description,
            location
        );

        await _context.Games.AddAsync(ett, cancellationToken);

        var gameParticipant = GameParticipant.Create(ett.Id, ett.CreatedBy, PlayerStatus.Accepted);
        await _context.GameParticipants.AddAsync(gameParticipant, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return res.SetSuccess(ett.ToViewDto());
    }

}

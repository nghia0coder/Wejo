using MediatR;

namespace Wejo.Game.Application.Commands;

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

        var ett = Game.Create(
            request.SportId,
            request.CreatedBy,
            request.SportFormatId,
            request.VenueId,
            request.GameTypeId,
            request.Area,
            request.Date,
            request.StartTime,
            request.EndTime,
            request.GameAccess,
            request.BringEquipment,
            request.CostShared,
            request.GameSkill,
            request.SkillStart,
            request.SkillEnd,
            request.TotalPlayer,
            request.Status,
            request.Description
        );

        await _context.Games.AddAsync(ett, cancellationToken);
        await _context.SaveChangesAsync(default);

        return res.SetSuccess(ett.ToViewDto());
    }

}

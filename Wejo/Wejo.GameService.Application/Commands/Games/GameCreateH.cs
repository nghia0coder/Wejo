using MediatR;
using Wejo.Common.Domain.Entities.Games.Extentions;
using Wejo.Common.Domain.Interfaces;
using Wejo.Common.SeedWork.Responses;
using Wejo.GameService.Application.Request.Games;

namespace Wejo.GameService.Application.Commands.Games
{
    public class GameCreateH : BaseH, IRequestHandler<GameCreateR, SingleResponse>
    {
        public GameCreateH(IWejoContext context) : base(context) { }

        public async Task<SingleResponse> Handle(GameCreateR request, CancellationToken cancellationToken)
        {
            var res = new SingleResponse();

            // ✅ Use the domain extension to create a Game
            var game = GameExtensions.CreateGame(
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

            _context.Games.Add(game);
            await _context.SaveChangesAsync(cancellationToken);

            return res.SetSuccess(200);
        }

    }
}

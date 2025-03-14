using Microsoft.EntityFrameworkCore;

namespace Wejo.Background.Job.Services;

using Common.Core.Enums;
using Common.Domain.Interfaces;
using Interfaces;

public class GameService : BaseService, IGameService
{
    public GameService(IWejoContext context) : base(context) { }

    public async Task UpdateGameStatusAsync()
    {
        var now = DateTime.UtcNow;

        await _context.Games
            .Where(g => g.Status == GameStatus.Waiting && g.StartTime <= now)
            .ExecuteUpdateAsync(setters => setters.SetProperty(g => g.Status, GameStatus.Playing));

        await _context.Games
            .Where(g => g.Status == GameStatus.Playing && g.EndTime <= now)
            .ExecuteUpdateAsync(setters => setters.SetProperty(g => g.Status, GameStatus.End));
    }
}

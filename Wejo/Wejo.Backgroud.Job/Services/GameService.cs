using Microsoft.EntityFrameworkCore;

namespace Wejo.Background.Job.Services;

using Common.Core.Enums;
using Common.Core.Extensions;
using Common.Domain.Entities;
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

        var gamesToEnd = await _context.Games
            .Where(g => g.Status == GameStatus.Playing && g.EndTime <= now)
            .Select(g => g.Id)
            .ToListAsync();

        if (gamesToEnd.Any())
        {
            await _context.Games
                .Where(g => gamesToEnd.Contains(g.Id))
                .ExecuteUpdateAsync(setters => setters.SetProperty(g => g.Status, GameStatus.End));

            foreach (var gameId in gamesToEnd)
            {
                await AddPlaypalsForGameAsync(gameId);
            }
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }

    private async Task AddPlaypalsForGameAsync(Guid gameId)
    {
        var participants = await _context.GameParticipants
                        .Where(p => p.GameId == gameId && p.Status == PlayerStatus.Accepted)
                        .Select(p => p.UserId)
                        .ToListAsync();

        if (participants.Count < 2) return;

        var existingPairs = new HashSet<string>(
                   await _context.UserPlaypals
                       .Where(p => p.GameId == gameId)
                       .Select(p => p.UserId1 + "-" + p.UserId2)
                       .ToListAsync()
               );

        var newPairs = new List<UserPlaypal>();

        for (int i = 0; i < participants.Count - 1; i++)
        {
            for (int j = i + 1; j < participants.Count; j++)
            {
                var userId1 = participants[i];
                var userId2 = participants[j];

                if (userId1.ToUid() > userId2.ToUid())
                {
                    (userId1, userId2) = (userId2, userId1);
                }

                var pairKey = userId1 + "-" + userId2;

                if (userId1 != userId2 && !existingPairs.Contains(pairKey))
                {
                    newPairs.Add(new UserPlaypal
                    {
                        UserId1 = userId1,
                        UserId2 = userId2,
                        GameId = gameId,
                        CreatedOn = DateTime.UtcNow
                    });

                    existingPairs.Add(pairKey);
                }
            }
        }

        if (newPairs.Count > 0)
        {
            _context.UserPlaypals.AddRange(newPairs);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}

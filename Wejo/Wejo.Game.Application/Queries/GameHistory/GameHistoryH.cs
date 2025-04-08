using MediatR;
using Microsoft.EntityFrameworkCore;
using Wejo.Common.Domain.Interfaces;
using Wejo.Common.SeedWork.Responses;
using Wejo.Game.Application.Request.Games;

namespace Wejo.Game.Application.Queries.GameSearch
{
    public class GameHistoryH : BaseH, IRequestHandler<GameHistoryR, SingleResponse>
    {
        public GameHistoryH(IWejoContext context) : base(context) { }

        public async Task<SingleResponse> Handle(GameHistoryR request, CancellationToken cancellationToken)
        {
            var res = new SearchResponse(request.PageNum, request.PageSize, request.Paging);
            var utcNow = DateTime.UtcNow;

            // Base query for games user has joined
            var baseQuery = _context.GameParticipants
                .Where(gp => gp.UserId == request.UserId)
                .Include(gp => gp.Game)
                    .ThenInclude(g => g.GameParticipants)
                .Select(gp => gp.Game)
                .AsQueryable();

            // Apply time filter
            if (request.TimeType?.ToLower() == "past")
            {
                baseQuery = baseQuery.Where(g => g.StartTime < utcNow);
            }
            else if (request.TimeType?.ToLower() == "upcoming")
            {
                baseQuery = baseQuery.Where(g => g.StartTime >= utcNow);
            }

            // Total records for paging
            res.TotalRecords = await baseQuery.CountAsync(cancellationToken);

            // Get paged games
            var skip = (request.PageNum - 1) * request.PageSize;
            var games = await baseQuery
                .OrderByDescending(g => g.StartTime)
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // Fetch distinct CreatedBy user IDs
            var createdByIds = games.Select(g => g.CreatedBy).Distinct().ToList();

            // Fetch user avatars
            var userAvatars = await _context.Users
                .Where(u => createdByIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Avatar, cancellationToken);

            // Map to DTO with custom fields
            var dtoList = games.Select(g =>
            {
                var dto = g.ToViewHistoryDto();
                dto.StartTime = g.StartTime;
                dto.EndTime = g.EndTime;
                dto.CurrentPlayer = g.GameParticipants.Count;
                dto.OwnerAvatar = userAvatars.ContainsKey(g.CreatedBy) ? userAvatars[g.CreatedBy] : null;
                return dto;
            }).ToList();

            return res.SetSuccess(dtoList);
        }
    }
}

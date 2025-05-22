using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Data;

namespace Wejo.Game.Application.Queries;

using Common.Domain.Interfaces;
using Common.SeedWork.Responses;
using Request;
using static Common.Domain.Entities.Game;

public class GameListInfoH : BaseH, IRequestHandler<GameListInfoR, SingleResponse>
{
    public GameListInfoH(IWejoContext context) : base(context) { }

    public async Task<SingleResponse> Handle(GameListInfoR request, CancellationToken cancellationToken)
    {
        var res = new SearchResponse(request.PageNum, request.PageSize, request.Paging);
        DateTime? startTime = request.Date?.ToDateTime(request.TimeStart ?? new TimeOnly(0, 0, 0));
        Point UserPoint = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

        string sqlQuery = "SELECT * FROM game.search_games(@Longitude,@Latitude,@SportId,@StartTime,@SkillStart,@SkillEnd,@PageSize,@Offset)";

        var parameters = new
        {
            Longitude = request.Longitude,
            Latitude = request.Latitude,
            SportId = (int)request.SportId,  // Explicitly convert to int
            StartTime = startTime,
            SkillStart = request.SkillStart.HasValue ? (int)request.SkillStart.Value : (int?)null, // Convert if not null
            SkillEnd = request.SkillEnd.HasValue ? (int)request.SkillEnd.Value : (int?)null,       // Convert if not null
            PageSize = (int)request.PageSize, // Convert ushort to int
            Offset = (int)((request.PageNum - 1) * request.PageSize) // Convert ushort to int
        };


        var connection = _context.Database.GetDbConnection();

        // Ensure connection is open
        if (connection.State == ConnectionState.Closed)
        {
            await connection.OpenAsync();
        }

        var query = await connection.QueryAsync<SearchDto>(sqlQuery, parameters);


        res.TotalRecords = query.Count();

        return res.SetSuccess(query);
    }

}

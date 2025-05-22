using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Queries;

using Common.Core.Extensions;
using Common.Domain.Dtos;
using Common.Domain.Interfaces;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Requests;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserViewH : BaseH, IRequestHandler<UserViewR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    public UserViewH(IWejoContext context) : base(context) { }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserViewR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new UserViewV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }

        #region -- Validate on server --
        var hasUser = await _context.Users.AnyAsync(p => p.Id == request.UserId, cancellationToken);
        if (!hasUser)
        {
            return res.SetError(nameof(E001), E001);
        }
        #endregion

        var fn = "identity.get_user_details";
        var @params = "@user_id";
        var paramValues = new { User_Id = request.UserId };

        UserDto result;

        using (var connection = _context.Database.GetDbConnection())
        {
            result = await connection.QuerySingleAsync<UserDto>(fn.ToFn("identity", "identity", @params), paramValues);
        }

        return res.SetSuccess(MapData(result));
    }

    private UserViewDto MapData(UserDto userView)
    {
        return new UserViewDto
        {
            Id = userView.Id,
            FullName = userView.FullName,
            Avatar = userView.Avatar,
            GenderName = userView.Gender.ToString(),
            Gender = userView.Gender,
            DateOfBirth = userView.DateOfBirth,
            LevelName = userView.Level.ToString(),
            Level = userView.Level,
            Bio = userView.Bio,
            GamesParticipatedCount = userView.GamesParticipatedCount,
            TotalPlaypal = userView.TotalPlaypal
        };
    }

    #endregion
}

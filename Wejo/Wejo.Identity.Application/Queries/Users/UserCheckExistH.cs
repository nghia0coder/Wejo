using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Commands;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Responses;
using Interfaces;
using Requests;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserCheckExistH : BaseSettingH, IRequestHandler<UserCheckExistR, SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    /// 
    public UserCheckExistH(IWejoContext context, ISetting setting) : base(context, setting)
    {
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserCheckExistR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new UserCheckExistV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }

        var hasUser = await _context.Users.AnyAsync(p => p.Id == request.Id);

        var data = new
        {
            isExist = hasUser,
        };

        return res.SetSuccess(data);
    }

    #endregion
}

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Queries;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
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
        var ett = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (ett == null)
        {
            return res.SetError(nameof(E001), E001);
        }
        #endregion

        return res.SetSuccess(ett.ToViewDto());
    }

    #endregion
}

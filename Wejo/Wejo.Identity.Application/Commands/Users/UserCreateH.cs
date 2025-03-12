using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Commands;

using Common.Core.Extensions;
using Common.Domain.Entities;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Interfaces;
using Requests;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserCreateH : BaseSettingH, IRequestHandler<UserCreateR, SingleResponse>
{
    #region --Fieds--

    private readonly IFirebaseAuthService _firebaseAuth;

    #endregion

    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    /// 
    public UserCreateH(IWejoContext context, ISetting setting, IFirebaseAuthService firebaseAuth) : base(context, setting)
    {
        _firebaseAuth = firebaseAuth;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserCreateR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new UserCreateV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }

        var userId = request.Id;
        if (userId == null)
        {
            return res.SetError(nameof(E119), E119);
        }

        #region -- Validate on server --
        var hasUser = await _context.Users.AnyAsync(p => p.Id == userId, cancellationToken);
        if (hasUser)
        {
            var t = new List<DicDto> { new() { Key = nameof(userId).ToCamelCase(), Value = userId } };
            return res.SetError(nameof(E002), E002, t);
        }
        #endregion

        var ett = User.Create(userId, request.FirstName, request.LastName, request.PhoneNumber, request.PhoneNumberConfirmed, request.Email, request.EmailConfirmed);

        await _context.Users.AddAsync(ett, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return res.SetSuccess(ett.ToViewDto());
    }

    #endregion
}

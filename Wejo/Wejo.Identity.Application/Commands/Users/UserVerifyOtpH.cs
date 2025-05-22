using MediatR;

namespace Wejo.Identity.Application.Commands;

using Common.Core.Extensions;
using Common.Domain.Interfaces;
using Common.SeedWork.Responses;
using Interfaces;
using Requests;
using Wejo.Common.Domain.Entities;
using Wejo.Identity.Application.Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserVerifyOtpH : BaseSettingH, IRequestHandler<UserVerifyOtpR, SingleResponse>
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
    public UserVerifyOtpH(IWejoContext context, ISetting setting, IFirebaseAuthService firebaseAuth) : base(context, setting)
    {
        _firebaseAuth = firebaseAuth;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserVerifyOtpR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new UserVerifyOtpV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }

        var data = await _firebaseAuth.VerifyOtpAsync(request.SessionId!, request.OtpCode!);

        if (data.IsNewUser)
        {
            User user = new User
            {
                Id = data.LocalId,
                PhoneNumber = data.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return res.SetSuccess(data);
    }

    #endregion
}

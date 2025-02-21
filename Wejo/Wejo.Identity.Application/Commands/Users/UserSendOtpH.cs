using MediatR;

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
public class UserSendOtpH : BaseSettingH, IRequestHandler<UserSendOtpR, SingleResponse>
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
    public UserSendOtpH(IWejoContext context, ISetting setting, IFirebaseAuthService firebaseAuth) : base(context, setting)
    {
        _firebaseAuth = firebaseAuth;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserSendOtpR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new UserSendOtpV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }

        var data = await _firebaseAuth.SendOtpAsync(request.PhoneNumber + "");

        return res.SetSuccess(data);
    }

    #endregion
}

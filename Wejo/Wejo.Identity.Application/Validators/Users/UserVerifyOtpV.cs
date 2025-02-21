using FluentValidation;

namespace Wejo.Identity.Application.Validators;

using Requests;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class UserVerifyOtpV : AbstractValidator<UserVerifyOtpR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserVerifyOtpV()
    {
        var t = "SessionInfo";
        RuleFor(p => p.SessionId).NotEmpty().WithMessage($"{t} {NotEmpty}");

        t = "OtpCode";
        RuleFor(p => p.OtpCode).NotEmpty().WithMessage($"{t} {NotEmpty}")
            .MinimumLength(OtpCode.Min).WithMessage($"{t} {MinimumLength} {OtpCode.Min}")
            .MaximumLength(OtpCode.Max).WithMessage($"{t} {MaximumLength} {OtpCode.Max}");
    }

    #endregion
}

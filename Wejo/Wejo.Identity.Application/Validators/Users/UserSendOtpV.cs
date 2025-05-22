using FluentValidation;

namespace Wejo.Identity.Application.Validators;

using Requests;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class UserSendOtpV : AbstractValidator<UserSendOtpR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserSendOtpV()
    {
        var t = "PhoneNumber";
        RuleFor(p => p.PhoneNumber).NotEmpty().WithMessage($"{t} {NotEmpty}")
            .MinimumLength(PhoneNumber.Min).WithMessage($"{t} {MinimumLength} {PhoneNumber.Min}")
            .MaximumLength(PhoneNumber.Max).WithMessage($"{t} {MaximumLength} {PhoneNumber.Max}")
            .Matches(PhoneNumber.Regex).WithMessage($"{t} {PhoneNumber.Message}");
    }

    #endregion
}

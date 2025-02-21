using FluentValidation;


namespace Wejo.Identity.Application.Validators;

using Requests;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class UserLoginSocialV : AbstractValidator<UserLoginSocialR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserLoginSocialV()
    {
        var t = "Email";
        RuleFor(p => p.Email).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

using FluentValidation;

namespace Wejo.Identity.Application.Validators;

using Requests;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class UserCheckExistV : AbstractValidator<UserCheckExistR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserCheckExistV()
    {
        var t = "Id";
        RuleFor(p => p.Id).NotEmpty().WithMessage($"{t} {NotEmpty}").Length(uid).WithMessage($"{t} {InvalidLength}");
    }

    #endregion
}

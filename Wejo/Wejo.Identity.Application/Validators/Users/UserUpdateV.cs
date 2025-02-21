using FluentValidation;

namespace Wejo.Identity.Application.Validators;

using Common.Core.Enums;
using Requests;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class UserUpdateV : AbstractValidator<UserUpdateR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserUpdateV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");

        // Validate Gender
        RuleFor(p => p.Gender)
            .Must(BeAValidGender)
            .When(p => !string.IsNullOrEmpty(p.Gender))
            .WithMessage("Gender must be one of: " + string.Join(", ", Enum.GetNames(typeof(Gender))));
    }

    /// <summary>
    /// Custom validator method to check gender enum
    /// </summary>
    private bool BeAValidGender(string? gender)
    {
        return Enum.TryParse(typeof(Gender), gender, true, out _);
    }

    #endregion
}

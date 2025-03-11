using FluentValidation;

namespace Wejo.Game.Application.Validators;

using Common.Core.Enums;
using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class GameParticipantUpdateV : AbstractValidator<GameParticipantUpdateR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public GameParticipantUpdateV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");

        RuleFor(p => p.PlayerStatus)
            .Must(BeAValidGender)
            .When(p => !string.IsNullOrEmpty(p.PlayerStatus))
            .WithMessage("Player status must be one of: " + string.Join(", ", Enum.GetNames(typeof(PlayerStatus))));
    }

    /// <summary>
    /// Custom validator method to check gender enum
    /// </summary>
    private bool BeAValidGender(string? gender)
    {
        return Enum.TryParse(typeof(PlayerStatus), gender, true, out _);
    }

    #endregion
}

using FluentValidation;

namespace Wejo.Game.Application.Validators;

using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class GameParticipantCreateV : AbstractValidator<GameParticipantCreateR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public GameParticipantCreateV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

using FluentValidation;

namespace Wejo.Game.Application.Validators;

using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class GameParticipantViewV : AbstractValidator<GameParticipantViewR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public GameParticipantViewV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

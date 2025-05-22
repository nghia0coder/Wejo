using FluentValidation;

namespace Wejo.Game.Application.Validators;

using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class GameViewV : AbstractValidator<GameViewR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public GameViewV()
    {
        var t = "GameId";
        RuleFor(p => p.Id).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

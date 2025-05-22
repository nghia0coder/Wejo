using FluentValidation;

namespace Wejo.Game.Application.Validators;

using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class GameChatMarkAsReadV : AbstractValidator<GameChatMarkAsReadR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public GameChatMarkAsReadV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");

        t = "GameId";
        RuleFor(p => p.Id).NotEmpty().WithMessage($"{t} {NotEmpty}");

        t = "MessageID";
        RuleFor(p => p.LastReadMessageId).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

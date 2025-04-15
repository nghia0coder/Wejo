using FluentValidation;

namespace Wejo.Game.Application.Validators;

using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class GameChatSendMessageV : AbstractValidator<GameChatSendMessageR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public GameChatSendMessageV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");

        t = "Message";
        RuleFor(p => p.Message).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

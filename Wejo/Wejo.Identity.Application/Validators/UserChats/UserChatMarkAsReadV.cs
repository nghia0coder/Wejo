using FluentValidation;

namespace Wejo.Identity.Application.Validators;

using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class UserChatMarkAsReadV : AbstractValidator<UserChatMarkAsReadR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserChatMarkAsReadV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");

        t = "UserId";
        RuleFor(p => p.Id).NotEmpty().WithMessage($"{t} {NotEmpty}");

        t = "MessageID";
        RuleFor(p => p.LastReadMessageId).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

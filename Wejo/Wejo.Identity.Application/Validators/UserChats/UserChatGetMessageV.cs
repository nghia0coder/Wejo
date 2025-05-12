using FluentValidation;

namespace Wejo.Identity.Application.Validators;

using Request;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class UserChatGetMessageV : AbstractValidator<UserChatGetMessageR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public UserChatGetMessageV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");
    }

    #endregion
}

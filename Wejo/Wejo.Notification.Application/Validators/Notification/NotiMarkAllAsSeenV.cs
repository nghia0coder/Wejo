using FluentValidation;

namespace Wejo.Notification.Application.Validators;

using Common.Core.Enums;
using Requests;
using static Common.SeedWork.Constants.Validator;

/// <summary>
/// Validator
/// </summary>
public class NotiMarkAllAsSeenV : AbstractValidator<NotiMarkAllAsSeenR>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public NotiMarkAllAsSeenV()
    {
        var t = "UserId";
        RuleFor(p => p.UserId).NotEmpty().WithMessage($"{t} {NotEmpty}");

        RuleFor(p => p.Type)
            .Must(BeAValidType)
            .When(p => !string.IsNullOrEmpty(p.Type))
            .WithMessage("Notification type must be one of: " + string.Join(", ", Enum.GetNames(typeof(NotificationType))));
    }

    /// <summary>
    /// Custom validator method to check type enum
    /// </summary>
    private bool BeAValidType(string? Type)
    {
        return Enum.TryParse(typeof(NotificationType), Type, true, out _);
    }

    #endregion
}

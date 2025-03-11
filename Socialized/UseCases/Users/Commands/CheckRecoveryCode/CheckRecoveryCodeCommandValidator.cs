using FluentValidation;

namespace UseCases.Users.Commands.CheckRecoveryCode;

public class CheckRecoveryCodeCommandValidator : AbstractValidator<CheckRecoveryCodeCommand>
{
    public CheckRecoveryCodeCommandValidator()
    {
        RuleFor(RuleFor => RuleFor.UserEmail)
            .NotEmpty().WithMessage("User email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(RuleFor => RuleFor.RecoveryCode)
            .NotEmpty().WithMessage("Recovery code is required")
            .InclusiveBetween(100000, 999999).WithMessage("Recovery code must be a 6-digit number");
    }
}

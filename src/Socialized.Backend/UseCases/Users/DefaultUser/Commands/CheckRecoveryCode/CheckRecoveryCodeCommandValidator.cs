using FluentValidation;

namespace UseCases.Users.DefaultUser.Commands.CheckRecoveryCode;

public class CheckRecoveryCodeCommandValidator : AbstractValidator<CheckRecoveryCodeCommand>
{
    public CheckRecoveryCodeCommandValidator()
    {
        RuleFor(RuleFor => RuleFor.Email)
            .NotEmpty().WithMessage("User email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(RuleFor => RuleFor.RecoveryCode)
            .NotEmpty().WithMessage("Recovery code is required")
            .InclusiveBetween(1000, 999999).WithMessage("Recovery code must be a from 4 to 6-digit number");
    }
}

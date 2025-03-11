using FluentValidation;

namespace UseCases.Admins.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.RecoveryCode)
            .NotEmpty().WithMessage("Recovery code is required")
            .InclusiveBetween(100000, 999999).WithMessage("Recovery code must be a 6-digit number");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password should be minimum 6 characters long");
    }
}
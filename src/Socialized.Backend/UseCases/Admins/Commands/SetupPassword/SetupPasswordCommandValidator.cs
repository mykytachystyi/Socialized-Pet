using FluentValidation;

namespace UseCases.Admins.Commands.SetupPassword;

public class SetupPasswordCommandValidator : AbstractValidator<SetupPasswordCommand>
{
    public SetupPasswordCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password should be minimum 6 characters long");
    }
}

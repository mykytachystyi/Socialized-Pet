using FluentValidation;

namespace UseCases.Users.DefaultUser.Commands.ChangePassword;

public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordValidator()
    {
        RuleFor(x => x.RecoveryToken)
            .NotEmpty().WithMessage("Recovery token is required");
        RuleFor(x => x.UserPassword)
            .NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.UserPassword)
            .Length(6, 100).WithMessage("Password should be minimum 6 characters long");
        RuleFor(x => x.UserConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required");
        RuleFor(x => x.UserConfirmPassword)
            .Equal(x => x.UserPassword).WithMessage("Password and confirmation password do not match");
    }
}

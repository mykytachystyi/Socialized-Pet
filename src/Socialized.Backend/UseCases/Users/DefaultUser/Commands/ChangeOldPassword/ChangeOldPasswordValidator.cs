using FluentValidation;

namespace UseCases.Users.DefaultUser.Commands.ChangeOldPassword;

public class ChangeOldPasswordValidator : AbstractValidator<ChangeOldPasswordCommand>
{
    public ChangeOldPasswordValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required");
        RuleFor(x => x.OldPassword)
            .Length(6, 100).WithMessage("Old password should be minimum 6 characters long");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required");
        RuleFor(x => x.NewPassword)
            .Length(6, 100).WithMessage("New password should be minimum 6 characters long");
    }
}

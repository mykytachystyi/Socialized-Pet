using FluentValidation;

namespace UseCases.Users.DefaultUser.Commands.RecoveryPassword;

public class RecoveryPasswordCommandValidator : AbstractValidator<RecoveryPasswordCommand>
{
    public RecoveryPasswordCommandValidator()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage("Email не може бути пустим.")
            .EmailAddress().WithMessage("Email не відповідає формату.");
    }
}

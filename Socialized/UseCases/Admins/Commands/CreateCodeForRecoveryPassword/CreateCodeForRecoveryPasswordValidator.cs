using FluentValidation;

namespace UseCases.Admins.Commands.CreateCodeForRecoveryPassword;

public class CreateCodeForRecoveryPasswordValidator : AbstractValidator<CreateCodeForRecoveryPasswordCommand>
{
    public CreateCodeForRecoveryPasswordValidator()
    {
        RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress()
            .WithMessage("Не правильний формат email-адреси.");
    }
}

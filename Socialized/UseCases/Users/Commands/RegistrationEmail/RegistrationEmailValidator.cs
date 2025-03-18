using FluentValidation;

namespace UseCases.Users.Commands.RegistrationEmail;

public class RegistrationEmailValidator : AbstractValidator<RegistrationEmailCommand>
{
    public RegistrationEmailValidator()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage("Email не може бути порожнім.")
            .EmailAddress().WithMessage("Email має бути валідним.");
        RuleFor(x => x.Culture)
            .NotEmpty().WithMessage("Культура не може бути порожньою.");
    }
}

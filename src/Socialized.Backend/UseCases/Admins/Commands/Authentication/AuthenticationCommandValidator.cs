using FluentValidation;

namespace UseCases.Admins.Commands.Authentication;

public class AuthenticationCommandValidator : AbstractValidator<AuthenticationCommand>
{
    public AuthenticationCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email не може бути порожнім.")
            .EmailAddress().WithMessage("Email має бути валідним.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль не може бути порожнім.")
            .MinimumLength(6).WithMessage("Пароль має містити мінімум 6 символів.");
    }
}
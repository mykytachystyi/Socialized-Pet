using FluentValidation;

namespace UseCases.Users.Commands.LogOut;

public class LogOutValidator : AbstractValidator<LogOutCommand>
{
    public LogOutValidator()
    {
        RuleFor(x => x.UserToken).NotEmpty().WithMessage("The user token can't be empty.");
    }
}
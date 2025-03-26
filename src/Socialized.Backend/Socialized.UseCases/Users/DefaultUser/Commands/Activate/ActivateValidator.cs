using FluentValidation;

namespace UseCases.Users.DefaultUser.Commands.Activate;

public class ActivateValidator : AbstractValidator<ActivateCommand>
{
    public ActivateValidator()
    {
        RuleFor(x => x.Hash).NotEmpty().WithMessage("The hash can't be empty.");
    }
}
using FluentValidation;

namespace UseCases.Users.Commands.Activate;

public class ActivateValidator : AbstractValidator<ActivateCommand>
{
    public ActivateValidator()
    {
        RuleFor(x => x.Hash).NotEmpty().WithMessage("The hash can't be empty.");
    }
}
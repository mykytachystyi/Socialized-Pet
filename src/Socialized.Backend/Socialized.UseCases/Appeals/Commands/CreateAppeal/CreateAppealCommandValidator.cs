using FluentValidation;

namespace UseCases.Appeals.Commands.CreateAppeal;

public class CreateAppealCommandValidator : AbstractValidator<CreateAppealCommand>
{
    public CreateAppealCommandValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(200).WithMessage("Subject cannot be longer than 200 characters");
    }
}

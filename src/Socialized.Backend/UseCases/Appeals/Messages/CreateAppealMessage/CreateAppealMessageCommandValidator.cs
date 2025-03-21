using FluentValidation;

namespace UseCases.Appeals.Messages.CreateAppealMessage;

public class CreateAppealMessageCommandValidator : AbstractValidator<CreateAppealMessageCommand>
{
    public CreateAppealMessageCommandValidator()
    {
        RuleFor(x => x.AppealId)
            .NotEmpty().WithMessage("Appeal ID is required");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(500).WithMessage("Message cannot be longer than 500 characters");
    }
}
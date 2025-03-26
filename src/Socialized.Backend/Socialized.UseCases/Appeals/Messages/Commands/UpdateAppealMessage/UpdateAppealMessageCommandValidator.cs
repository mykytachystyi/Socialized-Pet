using FluentValidation;

namespace UseCases.Appeals.Messages.Commands.UpdateAppealMessage;

public class UpdateAppealMessageCommandValidator : AbstractValidator<UpdateAppealMessageCommand>
{
    public UpdateAppealMessageCommandValidator()
    {
        RuleFor(x => x.MessageId)
            .NotEmpty().WithMessage("Message ID is required");
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(500).WithMessage("Message cannot be longer than 500 characters");
    }
}

using FluentValidation;

namespace UseCases.Appeals.Replies.Commands.CreateAppealMessageReply;

public class CreateAppealMessageReplyValidator : AbstractValidator<CreateAppealMessageReplyCommand>
{
    public CreateAppealMessageReplyValidator()
    {
        RuleFor(x => x.AppealMessageId)
            .NotEmpty().WithMessage("Appeal message ID is required");
        RuleFor(x => x.Reply)
            .NotEmpty().WithMessage("Reply is required");
        RuleFor(x => x.Reply)
            .MaximumLength(500).WithMessage("Reply cannot be longer than 500 characters");
    }
}
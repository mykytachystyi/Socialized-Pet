using FluentValidation;

namespace UseCases.Appeals.Replies.Commands.UpdateAppealMessageReply;

public class UpdateAppealMessageReplyValidator : AbstractValidator<UpdateAppealMessageReplyCommand>
{
    public UpdateAppealMessageReplyValidator()
    {
        RuleFor(x => x.ReplyId)
            .NotEmpty().WithMessage("Reply ID is required");
        RuleFor(x => x.Reply)
            .NotEmpty().WithMessage("Reply is required");
        RuleFor(x => x.Reply)
            .MaximumLength(500).WithMessage("Reply cannot be longer than 500 characters");
    }
}
